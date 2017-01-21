using UnityEngine;
using System.Collections.Generic;

public class AggroScript : MonoBehaviour {

    public string m_aggroTargetTag = "Player";

    [SerializeField] private bool m_aggroed;
    [SerializeField] private Animator m_animator;
    private Transform m_aggroTarget;
    private List<Player> m_players;

	// Use this for initialization
	void Start ()
    {
        m_aggroed = false;
        m_players = new List<Player>();
        GameObject[] players = GameObject.FindGameObjectsWithTag(m_aggroTargetTag);
        foreach(GameObject player in players)
        {
            Player ws = player.GetComponent<Player>();
            if(ws)
            {
                m_players.Add(ws);
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        foreach(Player player in m_players)
        {
            if(player.IsWaving())
            {
                if (m_aggroed)
                {
                    if (Vector2.Distance(transform.position, player.gameObject.transform.position) < Vector2.Distance(transform.position, m_aggroTarget.position))
                    {
                        m_aggroTarget = player.gameObject.transform;
                    }
                }
                else
                {
                    m_aggroTarget = player.gameObject.transform;
                }
                m_aggroed = true;
            }
        }
	}

    void OnTriggerEnter2D(Collider2D _col)
    {
        if (!m_aggroed)
        {
            if (_col.tag == m_aggroTargetTag)
            {
                float dist = Vector2.Distance(transform.position, _col.transform.position);
                Vector2 dir = _col.transform.position - transform.position;
                dir = Vector3.Normalize(dir);
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, dist);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.tag == m_aggroTargetTag)
                    { 
                        m_aggroed = true;
                        m_aggroTarget = _col.gameObject.transform;
                    }
                }
            }
        }
    }

    public bool IsAggroed()
    {
        return m_aggroed;
    }

    public Transform GetTarget()
    {
        return m_aggroTarget;
    }
}
