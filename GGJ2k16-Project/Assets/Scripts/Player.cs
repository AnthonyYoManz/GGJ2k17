using UnityEngine;
using System.Collections.Generic;


public class Player : MonoBehaviour
{
    public bool m_ignoreInput;
    //Bit dirty but oh well!
    public CameraScript m_cam;
    public int m_playerID;
    public enum STATE { DEFAULT, WAVING, DEAD, NO_STATE };
    public enum ANIM_STATE { IDLE, LEFT, RIGHT, UP, DOWN, WAVING };

    public STATE m_curState;

    public int m_lives = 3;
    public float m_onHitInvulDuration = 1.0f;
    private float m_invulTimer;

    [SerializeField]
    private float m_moveSpeed;
    private STATE m_prevState;
    private Rigidbody2D m_rBody;
    [SerializeField]
    private Collider2D m_collider;
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private float m_animationSpeed;

    private Rigidbody2D m_rb;
    private List<GameObject> m_interactables;
    SpriteRenderer m_sprRend;

    private ANIM_STATE m_curMoveState;

    public GameObject m_interactionIndicator;
    public float m_interactionIndicatorYOffset = 3.8f;

    public Transform m_otherTransform;
    public int m_minSortingLayer = 6;


    public AudioClip m_waveSound;
    public AudioClip m_deathSound;
    private AudioSource m_audioSource;

    private Timer m_waveTimer;
    [SerializeField]
    private float m_minWaveDuration;

    public float GetMaxMoveSpeed()
    {
        return m_moveSpeed;
    }
    public bool MoveTowards(Vector3 target, float maxSpeed)
    {
        Vector3 prevPos = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, target, maxSpeed);

        Vector3 dir = (transform.position - prevPos).normalized;
        ApplyAnimation(dir);

        return transform.position == target;
    }

    public void ForceAnimation(ANIM_STATE state)
    {
        if (m_curMoveState != state)
        {
            //Wish i'd just mapped this KILL ME
            switch (state)
            {
                case ANIM_STATE.IDLE: m_animator.SetTrigger("IDLE"); break;
                case ANIM_STATE.DOWN: m_animator.SetTrigger("MOVE_DOWN"); break;
                case ANIM_STATE.UP: m_animator.SetTrigger("MOVE_UP"); break;
                case ANIM_STATE.LEFT: m_animator.SetTrigger("MOVE_LEFT"); break;
                case ANIM_STATE.RIGHT: m_animator.SetTrigger("MOVE_RIGHT"); break;
                case ANIM_STATE.WAVING: m_animator.SetTrigger("WAVE"); break;
            }

            m_curMoveState = state;
        }
    }

    public void ForceSetAnimSpeed(float speed)
    {
        m_animator.SetFloat("SPEED", speed);
    }

    public void ForceSetDefaultAnimSpeed()
    {
        m_animator.SetFloat("SPEED", m_animationSpeed);
    }

    void Awake()
    {
        //most Component refs set up in inspector
        m_audioSource = GetComponent<AudioSource>();
    }

	// Use this for initialization
	void Start ()
    {
        m_waveTimer = new Timer();
        m_invulTimer = m_onHitInvulDuration;
        m_prevState = STATE.NO_STATE;
        m_curState = STATE.DEFAULT;
        m_sprRend = GetComponent<SpriteRenderer>();
        m_rb = GetComponent<Rigidbody2D>();
        m_interactables = new List<GameObject>();

        if (m_animator)
        {

            m_animator.SetFloat("SPEED", 0.0f);
            m_curMoveState = ANIM_STATE.IDLE;
            m_animator.SetTrigger("IDLE");//Unless there is also an idle anim..
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_curState != STATE.DEAD)
        {
            if (m_invulTimer < m_onHitInvulDuration)
            {
                m_invulTimer += Time.deltaTime;
                if (m_sprRend)
                {
                    m_sprRend.color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                }
            }
            else
            {
                if (m_sprRend)
                {
                    m_sprRend.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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

        Vector3 pos = transform.position;
        Vector3 oPos = m_otherTransform.position;
        if (pos.y < oPos.y)
        {
            m_sprRend.sortingOrder = m_minSortingLayer + 1;
        }
        else
        {
            m_sprRend.sortingOrder = m_minSortingLayer;
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
        if (!m_ignoreInput)
        {
            ApplyAnimation(velocity.normalized);
        }
        else
        {
            velocity = Vector2.zero;
        }

        bool actionPressed = Input.GetButtonDown("p" + m_playerID + "Action");
        GameObject interactable;

        InteractableScript iscript;

        if (m_interactables.Count > 0)
        {
            interactable = m_interactables[0];
            iscript = interactable.GetComponent<InteractableScript>();
            Vector3 intPos = interactable.transform.position;
            intPos.y += m_interactionIndicatorYOffset;
            m_interactionIndicator.transform.position = intPos;
            if (iscript)
            {
                if (iscript.IsInteractable())
                {
                    if (!m_interactionIndicator.activeSelf)
                    {
                        m_interactionIndicator.SetActive(true);
                    }
                    if (actionPressed)
                    {
                        iscript.Interact();
                    }
                }
                if (!iscript.IsInteractable())
                {
                    m_interactables.Remove(iscript.gameObject);
                }
            }
        }
        else
        {
            if (m_interactionIndicator.activeSelf)
            {
                m_interactionIndicator.SetActive(false);
            }
            if(actionPressed)
            {
                m_curState = STATE.WAVING;
            }
        }

        Vector2 pos = m_rb.transform.position;
        pos += velocity * Time.deltaTime;
        m_rb.MovePosition(pos);
    }

    private void ApplyAnimation(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            m_animator.SetFloat("SPEED", m_animationSpeed);
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0 && m_curMoveState != ANIM_STATE.RIGHT)
                {
                    m_animator.SetTrigger("MOVE_RIGHT");
                    m_curMoveState = ANIM_STATE.RIGHT;
                }
                else if (direction.x < 0 && m_curMoveState != ANIM_STATE.LEFT)
                {
                    m_animator.SetTrigger("MOVE_LEFT");
                    m_curMoveState = ANIM_STATE.LEFT;
                }
            }
            else //if moving up/down
            {
                if (direction.y > 0 && m_curMoveState != ANIM_STATE.UP)
                {
                    m_animator.SetTrigger("MOVE_UP");
                    m_curMoveState = ANIM_STATE.UP;
                }
                else if (direction.y < 0 && m_curMoveState != ANIM_STATE.DOWN)//if mostly DOWN
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

    void MovingEnd()
    {

    }

    void WavingTransition()
    {
        if (m_animator && m_curMoveState != ANIM_STATE.WAVING && !m_ignoreInput)
        {
            m_animator.SetFloat("SPEED", m_animationSpeed);
            m_animator.SetTrigger("WAVE");
            m_curMoveState = ANIM_STATE.WAVING;
            m_waveTimer.Restart();

            if (m_waveSound && m_audioSource) m_audioSource.PlayOneShot(m_waveSound);
        }
    }

    void WavingUpdate()
    {
        if (m_waveTimer.Elapsed() >= m_minWaveDuration && !Input.GetButton("p" + m_playerID + "Action"))
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
            Debug.Log("OUCH! Who's been programming by copying and pasting code?");
            m_lives--;
            if (m_lives <= 0)
            {
                m_curState = STATE.DEAD;
                if (m_deathSound && m_audioSource) m_audioSource.PlayOneShot(m_deathSound);
                FlameIRLPlayer();
                m_sprRend.color = new Color(0, 0, 0, 0);
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
