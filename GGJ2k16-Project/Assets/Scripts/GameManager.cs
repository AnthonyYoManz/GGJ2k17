using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager   s_singleton;
    public WorldGridScript      m_worldGrid { get; private set; }

    private Player[]            m_players;
    private CameraScript        m_cameraScript;
    private Vector2[]           m_playerPositions;


    public Player[] GetPlayers()
    {
        return m_players;
    }

	// Use this for initialization
	void Awake ()
    {
        Debug.Assert(s_singleton == null, "Only one game manager allowed");
        s_singleton = this;
        //DontDestroyOnLoad(this.gameObject);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        m_players = new Player[players.Length];
        for (int i = 0; i < players.Length; ++i) m_players[i] = players[i].GetComponent<Player>();

        m_cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
        m_worldGrid = GameObject.Find("GameWorldGrid").GetComponent<WorldGridScript>();
        Debug.Assert(m_worldGrid, "Uh oh can't find a world grid script. name the bject GameWorldGrid for me.");
    }

    void Start()
    {
       
        //change room changing to go through this script 
    }

    // Update is called once per frame
    void Update ()
    {
        foreach (Player p in m_players)
        {
            //if (p.m_curState == Player.STATE.DEAD)
            {
                //if input == restart
                if (false)
                {
                    RestartLevel();
                }
            }
        }

	}

    public void Win()
    {
        Debug.Log("winnnnnnnnnnerrrs");
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        int idx = 0;
        foreach (Player p in m_players)
        {
            p.transform.position = m_playerPositions[++idx];
        }
    }

    public void AllowPlayerInput(bool b)
    {
        foreach (Player p in m_players)
        {
            p.m_ignoreInput = !b;
        }
    }

}
