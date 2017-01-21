using UnityEngine;
using System.Collections.Generic;


public class Player : MonoBehaviour
{
    //Bit dirty but oh well!
    public CameraScript         m_cam;
    public int m_playerID;
    public enum STATE { DEFAULT, WAVING, NO_STATE };
    public STATE                m_curState;

    [SerializeField]
    private float               m_moveSpeed;
    private STATE               m_prevState;        
    private Rigidbody2D         m_rBody;
    [SerializeField]
    private Collider2D          m_collider;
    [SerializeField]
    private Animator            m_animator;
    [SerializeField]
    private float               m_animationSpeed;

    private Rigidbody2D         m_rb;
    private List<GameObject>    m_interactables;

    void Awake()
    {
        //Component refs set up in inspector

    }

	// Use this for initialization
	void Start ()
    {
        m_prevState = STATE.NO_STATE;
        m_curState = STATE.DEFAULT;

        m_rb = GetComponent<Rigidbody2D>();
        m_interactables = new List<GameObject>();
        if (m_animator)
        {
            m_animator.SetFloat("SPEED", m_animationSpeed);
            m_animator.SetTrigger("MOVE_RIGHT");//Unless there is also an idle anim..
        }
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
        Vector2 velocity = new Vector2(Input.GetAxis("p" + m_playerID + "Horizontal"), Input.GetAxis("p" + m_playerID + "Vertical"));
        //if input is not large enough (IE in dead zone, set velocity to vector2.zero)
        velocity *= m_moveSpeed;

        //Moving sideways
        if (m_animator)
        {
            if (velocity != Vector2.zero)
            {
                if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
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
                    if (velocity.y >= 0)
                    {
                        m_animator.SetTrigger("MOVE_UP");
                    }
                    else //if mostly DOWN
                    {
                        m_animator.SetTrigger("MOVE_DOWN");
                    }
                }
            }
        }

        if (Input.GetButton("p" + m_playerID + "Action"))
        {
            if (m_interactables.Count == 0)
            {
                m_curState = STATE.WAVING;
            }
            else
            {
                GameObject interactable = m_interactables[0];
                InteractableScript iscript = interactable.GetComponent<InteractableScript>();
                if(iscript)
                {
                    iscript.Interact();
                }
            }
        }

        Vector2 pos = m_rb.transform.position;
        pos += velocity * Time.deltaTime;
        m_rb.MovePosition(pos);
    }

    void MovingEnd()
    {

    }

    void WavingTransition()
    {
        if (m_animator)
        {
            m_animator.SetTrigger("WAVE");
        }
    }

    void WavingUpdate()
    {
        if(Input.GetButtonUp("p" + m_playerID + "Action"))
        {
            m_curState = STATE.DEFAULT;
        }
    }

    void WavingEnd()
    {

    }

    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.tag == "Interactable")
        {
            m_interactables.Add(_col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D _col)
    {
        if (_col.tag == "Interactable")
        {
            m_interactables.Remove(_col.gameObject);
        }
    }

    public bool IsWaving()
    {
        return m_curState == STATE.WAVING;
    }
}
