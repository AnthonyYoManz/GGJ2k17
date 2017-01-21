﻿using UnityEngine;
using System.Collections.Generic;


public class Player : MonoBehaviour
{
    //Bit dirty but oh well!
    public CameraScript         m_cam;
    public int m_playerID;
    public enum STATE { DEFAULT, WAVING, DEAD, NO_STATE };
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
            m_animator.SetTrigger("MOVE_RIGHT");//Unless there is also an idle anim..
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
