using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AggroScript))]
public class EnemyAmbleScript : MonoBehaviour {
    public bool m_flipWhileAmbling = false;
    public float m_ambleRadius = 3.0f;
    public float m_ambleSpeed = 5.0f;
    public Animator m_animator;
    private Vector3 m_spawnPos;
    private Vector3 m_targetPos;
    private AggroScript m_aggroScript;

	// Use this for initialization
	void Start ()
    {
        m_aggroScript = GetComponent<AggroScript>();
        m_spawnPos = transform.position;
        GetRandomTargetPos();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(m_aggroScript)
        {
            if(!m_aggroScript.IsAggroed())
            {
                transform.position = Vector3.MoveTowards(transform.position, m_targetPos, m_ambleSpeed * Time.deltaTime);
                if (m_animator && m_flipWhileAmbling)
                {
                    Vector2 direction = m_targetPos - transform.position;
                    if (direction.x > 0)
                    {
                        m_animator.SetTrigger("MOVE_RIGHT");
                    }
                    else
                    {
                        m_animator.SetTrigger("MOVE_LEFT");
                    }
                }
                if (transform.position == m_targetPos)
                {
                    GetRandomTargetPos();
                }
            }
        }
	}
    
    private void GetRandomTargetPos()
    {
        m_targetPos = m_spawnPos + (Vector3)(Random.insideUnitCircle * m_ambleRadius);
        m_targetPos.z = m_spawnPos.z;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.4f, 0.4f, 1.0f, 0.8f);
        Gizmos.DrawWireSphere(m_spawnPos, m_ambleRadius);
    }
}
