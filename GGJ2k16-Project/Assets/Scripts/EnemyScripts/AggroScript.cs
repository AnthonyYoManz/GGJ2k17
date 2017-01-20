using UnityEngine;
using System.Collections.Generic;

public class AggroScript : MonoBehaviour {

    public string m_aggroTargetTag = "Player";
    public float m_speed = 10.0f;

    [SerializeField] private bool m_aggroed;
    private Transform m_aggroTarget;
    //private List<WaveScript> m_players;

	// Use this for initialization
	void Start () {
        m_aggroed = false;
        GameObject[] players = GameObject.FindGameObjectsWithTag(m_aggroTargetTag);
        foreach(GameObject player in players)
        {
            /*WaveScript ws = player.GetComponent<WaveScript>();
            if(ws)
            {
                m_players.Add(ws);
            }*/
        }
	}
	
	// Update is called once per frame
	void Update () {
        /*foreach(WaveScript player in m_players)
        {
            if(player.IsWaving())
            {
                m_aggroed = true;
                m_aggroTarget = player.gameObject.transform;
            }
        }*/
        if(m_aggroed)
        {
            Vector3 newPos = transform.position;
            Vector3 dir = Vector3.Normalize(m_aggroTarget.position - newPos);
            Debug.Log(dir);
            newPos += dir * m_speed * Time.deltaTime;
            transform.position = newPos;
        }
	}

    void OnTriggerEnter2D(Collider2D _col)
    {
        if(_col.tag == m_aggroTargetTag)
        {   
            float dist = Vector2.Distance(transform.position, _col.transform.position);
            Vector2 dir = _col.transform.position - transform.position;
            dir = Vector3.Normalize(dir);
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, dist);
            foreach(RaycastHit2D hit in hits)
            { 
                Debug.Log(hit.collider.tag);
                if (hit.collider.tag == m_aggroTargetTag)
                {
                    Debug.Log("Double yee");
                    m_aggroed = true;
                    m_aggroTarget = _col.gameObject.transform;
                }
            }
        }
    }
}
