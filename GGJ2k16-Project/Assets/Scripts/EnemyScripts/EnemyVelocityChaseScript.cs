using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AggroScript))]
public class EnemyVelocityChaseScript : MonoBehaviour {
    [SerializeField]
    private Animator m_animator;

    private AggroScript m_aggro;

    public float m_maxSpeed = 13.0f;
    public float m_acceleration = 30.0f;
    public float m_deceleration = 15.0f;

    private Vector2 m_velocity;
    private Vector2 m_direction;


    // Use this for initialization
    void Start()
    {
        m_aggro = GetComponent<AggroScript>();
        Debug.Assert(m_aggro);
        m_velocity = new Vector2(0, 0);
        m_direction = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if (m_aggro.IsAggroed())
        {
            if (m_aggro.GetTarget().position - pos != Vector3.zero)
            {
                m_direction = Vector3.Normalize(m_aggro.GetTarget().position - pos);
                if (m_animator)
                {
                    if (m_direction.x > 0)
                    {
                        m_animator.SetTrigger("MOVE_RIGHT");
                    }
                    else
                    {
                        m_animator.SetTrigger("MOVE_LEFT");
                    }
                }
            }

        }
        m_velocity += m_acceleration * m_direction * Time.deltaTime;
        if (m_velocity != Vector2.zero)
        {
            m_velocity -= m_deceleration * m_velocity.normalized * Time.deltaTime;
        }
        pos += (Vector3)m_velocity * Time.deltaTime;
        transform.position = pos;
    }
}
