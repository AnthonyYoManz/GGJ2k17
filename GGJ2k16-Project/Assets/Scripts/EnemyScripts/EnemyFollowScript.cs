using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AggroScript))]
public class EnemyFollowScript : MonoBehaviour {

    [SerializeField]
    private Animator m_animator;

    private AggroScript m_aggro;

    public float m_speed = 10.0f;

	// Use this for initialization
	void Start () {
        m_aggro = GetComponent<AggroScript>();
        Debug.Assert(m_aggro);
	}
	
	// Update is called once per frame
	void Update () {
	    if(m_aggro.IsAggroed())
        {
            Vector3 pos = transform.position;
            if (m_aggro.GetTarget().position - pos != Vector3.zero)
            {
                Vector3 dir = Vector3.Normalize(m_aggro.GetTarget().position - pos);
                if (m_animator)
                {
                    if (dir.x > 0)
                    {
                        m_animator.SetTrigger("MOVE_RIGHT");
                    }
                    else
                    {
                        m_animator.SetTrigger("MOVE_LEFT");
                    }
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, m_aggro.GetTarget().position, m_speed * Time.deltaTime);
        }
	}
}
