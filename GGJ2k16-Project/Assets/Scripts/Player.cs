using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour
{ 
    public int m_playerID;
    public enum STATE { DEFAULT, WAVING, NO_STATE };
    public STATE                m_curState;

    [SerializeField]
    private float               m_moveSpeed;
    private STATE               m_prevState;        
    private Rigidbody2D         m_rBody;
    [SerializeField]
    private CircleCollider2D    m_waveTrigger;
    [SerializeField]
    private Collider2D          m_collider;
    [SerializeField]
    private Animator            m_animator;
    [SerializeField]
    private float               m_animationSpeed;

    void Awake()
    {
        //Component refs set up in inspector

    }

	// Use this for initialization
	void Start ()
    {
        m_prevState = STATE.NO_STATE;
        m_curState = STATE.DEFAULT;

        
        m_animator.SetFloat("SPEED", m_animationSpeed);
        m_animator.SetTrigger("MOVE_RIGHT");//Unless there is also an idle anim..
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_curState != m_prevState)
        {
            switch (m_prevState)
            {
                case STATE.DEFAULT: MovingEnd(); break;
                case STATE.WAVING: WavingEnd(); break;
            }
            switch (m_curState)
            {
                case STATE.DEFAULT: MovingTransition(); break;
                case STATE.WAVING: WavingTransition(); break;
            }
        }
        m_prevState = m_curState;

        
        switch (m_curState)
        {
            case STATE.DEFAULT: MovingUpdate(); break;
            case STATE.WAVING: WavingUpdate(); break;
        }
	}



    void MovingTransition()
    {
        
    }
    void MovingUpdate()
    {
        //some intput
        //get axis input
        Vector2 velocity = Vector2.zero; //= input axis() @Axis input
        //if input is not large enough (IE in dead zone, set velocity to vector2.zero)
        velocity *= m_moveSpeed;

        //Moving sideways
        if (velocity.x > velocity.y)
        {
            if (velocity.x >= 0)
            {
                m_animator.SetTrigger("MOVE_RIGHT");
            }
            else //if mostly moving UP
            {
                m_animator.SetTrigger("MOVE_LEFT");
            }
        }
        else //if moving up/down
        {
            if (velocity.y >=0)
            {
                m_animator.SetTrigger("MOVE_UP");
            }
            else //if mostly DOWN
            {
                m_animator.SetTrigger("MOVE_DOWN");
            }
        }
    
        //@Waving input
        //
        if(Input.GetButton("Jump"))
        {
            m_curState = STATE.WAVING;
        }
    }

    void MovingEnd()
    {

    }

    void WavingTransition()
    {
        m_waveTrigger.enabled = true;
        m_animator.SetTrigger("WAVING");
    }

    void WavingUpdate()
    {
        //If waving input stopped @ Waving input
        {
            m_curState = STATE.DEFAULT;
        }
    }

    void WavingEnd()
    {
        m_waveTrigger.enabled = true;
    }

    public bool IsWaving()
    {
        return m_curState == STATE.WAVING;
    }
}
