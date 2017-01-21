using UnityEngine;
using System.Collections.Generic;


public class Player : MonoBehaviour
{
    //Bit dirty but oh well!
    public CameraScript         m_cam;
    public int m_playerID;
    public enum STATE { DEFAULT, WAVING, DEAD, NO_STATE };
    public enum ANIM_STATE { IDLE, LEFT, RIGHT, UP, DOWN, WAVING };

    public STATE                m_curState;

    public int m_lives = 3;
    public float m_onHitInvulDuration = 1.0f;
    private float m_invulTimer;

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
    SpriteRenderer sprRend;

    private ANIM_STATE m_curMoveState;

    public GameObject m_interactionIndicator;
    public float m_interactionIndicatorYOffset = 3.8f;


    void Awake()
    {
        //Component refs set up in inspector

    }

	// Use this for initialization
	void Start ()
    {
        m_invulTimer = m_onHitInvulDuration;
        m_prevState = STATE.NO_STATE;
        m_curState = STATE.DEFAULT;
        sprRend = GetComponent<SpriteRenderer>();
        m_rb = GetComponent<Rigidbody2D>();
        m_interactables = new List<GameObject>();

        if (m_animator)
        {
            m_animator.SetFloat("SPEED", m_animationSpeed);
            m_curMoveState = ANIM_STATE.IDLE;
            m_animator.SetTrigger("IDLE");//Unless there is also an idle anim..
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_curState != STATE.DEAD)
        {
            if (m_invulTimer < m_onHitInvulDuration)
            {
                m_invulTimer += Time.deltaTime;
                if (sprRend)
                {
                    sprRend.color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                }
            }
            else
            {
                if (sprRend)
                {
                    sprRend.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
            }
        }

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
                m_animator.SetFloat("SPEED", 1.0f);
                if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
                {
                    if (velocity.x >= 0 && m_curMoveState != ANIM_STATE.RIGHT)
                    {
                        m_animator.SetTrigger("MOVE_RIGHT");
                        m_curMoveState = ANIM_STATE.RIGHT;
                    }
                    else if (velocity.x < 0 &&  m_curMoveState != ANIM_STATE.LEFT)
                    {
                        m_animator.SetTrigger("MOVE_LEFT");
                        m_curMoveState = ANIM_STATE.LEFT;
                    }
                }
                else //if moving up/down
                {
                    if (velocity.y >= 0 && m_curMoveState != ANIM_STATE.UP)
                    {
                        m_animator.SetTrigger("MOVE_UP");
                        m_curMoveState = ANIM_STATE.UP;
                    }
                    else if (velocity.y < 0 && m_curMoveState != ANIM_STATE.DOWN)//if mostly DOWN
                    {
                        m_curMoveState = ANIM_STATE.DOWN;
                        m_animator.SetTrigger("MOVE_DOWN");
                    }
                }
            }
            else if (m_curMoveState != ANIM_STATE.IDLE)//if not moving
            {
                m_curMoveState = ANIM_STATE.IDLE;
                //m_animator.SetTrigger("IDLE");
                m_animator.SetFloat("SPEED", 0.0f);
            }
        }

        if (m_interactables.Count > 0)
        {
            GameObject interactable = m_interactables[0];
            Vector3 intPos = interactable.transform.position;
            intPos.y += m_interactionIndicatorYOffset;
            m_interactionIndicator.transform.position = intPos;
            if(!m_interactionIndicator.activeSelf)
            {
                m_interactionIndicator.SetActive(true);
            }
        }
        else
        {
            if (m_interactionIndicator.activeSelf)
            {
                m_interactionIndicator.SetActive(false);
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
        if (m_animator && m_curMoveState != ANIM_STATE.WAVING)
        {
            m_animator.SetFloat("SPEED", 1.0f);
            m_animator.SetTrigger("WAVE");
            m_curMoveState = ANIM_STATE.WAVING;
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

    void OnCollisionEnter2D(Collision2D _col)
    {
        if(_col.gameObject.tag == "Enemy")
        {
            
        }
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
        return (m_curState == STATE.WAVING);
    }

    public bool IsInvulnerable()
    {
        return (m_invulTimer < m_onHitInvulDuration);
    }

    public void OnHit()
    {
        if (!IsInvulnerable())
        {
            Debug.Log("OUCH! Who's been programming by copying and pasting?");
            m_lives--;
            if (m_lives <= 0)
            {
                m_curState = STATE.DEAD;
                FlameIRLPlayer();
                sprRend.color = new Color(0, 0, 0, 0);
            }
            m_invulTimer = 0.0f;
        }
    }

    public void FlameIRLPlayer()
    {
        int randy = Random.Range(0, 5);
        switch(randy)
        {
            case 0:
                Debug.Log("ur shit mate");
                break;
            default:
                Debug.Log("You died");
                break;
        }
    }
}
