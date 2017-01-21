using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSwapScript : MonoBehaviour {

    public Player[] m_players = new Player[2];
    public float m_timeBeforeGameOver = 3.0f;
    private float m_goTimer;
    private bool m_triggered;

	// Use this for initialization
	void Start () {
        m_goTimer = 0;
        m_triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_triggered)
        {
            m_goTimer += Time.deltaTime;
            if(m_goTimer >= m_timeBeforeGameOver)
            {
                //do gameover party
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                m_triggered = false;
            }
        }
        foreach (Player player in m_players)
        {
            if(player.IsDead() )
            {
                TriggerTimer();
            }
        }
	}

    private void TriggerTimer()
    {
        if(!m_triggered)
        {
            m_triggered = true;
            m_goTimer = 0;
        }
    }
}
