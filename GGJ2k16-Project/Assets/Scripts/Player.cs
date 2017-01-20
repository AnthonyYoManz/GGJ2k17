using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour
{ 
    public int m_playerID;
    public enum STATE { DEFAULT, WAVING, NO_STATE };
    public STATE                m_curState;
    
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


    }

	// Use this for initialization
	void Start ()
    {

        m_animator.SetFloat("SPEED", m_animationSpeed);
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
        //apply some motion
        //LEFT
        {
            m_animator.SetTrigger("MOVE_LEFT");
        }
        //RIGHT
        {
            m_animator.SetTrigger("MOVE_RIGHT");
        }

        //wave
        //
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
        //If waving input stopped
        {
            m_curState = STATE.DEFAULT;
        }
    }

    void WavingEnd()
    {
        m_waveTrigger.enabled = true;
    }
}
