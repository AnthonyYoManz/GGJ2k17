using UnityEngine;
using System.Collections.Generic;

public class AggroScript : MonoBehaviour {

    public string m_aggroTargetTag = "Player";

    public float m_searchRadius = 10.0f;

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
        bool targetAcquired = false;
        foreach(Player player in m_players)
        {
            if(player.IsWaving())
            {
                if (targetAcquired)
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
                targetAcquired = true;
                m_aggroed = true;
            }

            if (!m_aggroed)
            {
                float dist = Vector2.Distance(transform.position, player.gameObject.transform.position);
                if (dist <= m_searchRadius)
                { 
                    Vector2 dir = player.gameObject.transform.position - transform.position;
                    dir = Vector3.Normalize(dir);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, dist);
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider.tag == m_aggroTargetTag)
                        {
                            m_aggroed = true;
                            m_aggroTarget = player.gameObject.transform;
                        }
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D _col)
    {
        if(_col.tag == "Player")
        {
            Player player = _col.gameObject.GetComponent<Player>();
            if(player)
            {
                player.OnHit();
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

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, m_searchRadius);
    }
}
