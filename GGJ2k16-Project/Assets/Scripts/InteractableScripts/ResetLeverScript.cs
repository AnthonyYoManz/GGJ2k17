using UnityEngine;
using System.Collections;

public class ResetLeverScript : LeverScript
{
    public Sprite m_defaultSprite;
    public float m_timeBeforeReset;
    private Timer m_timer;
	void Start ()
    {
        m_timer = new Timer();
        m_interacted = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_interacted && m_timer.Elapsed() >= m_timeBeforeReset && m_triggerable != null && !m_triggerable.Triggered())
        {
            m_triggerable.UndoTrigger();
            m_interacted = false;
            if (m_defaultSprite)
            {
                SpriteRenderer sprRnd = GetComponent<SpriteRenderer>();
                if (sprRnd)
                {
                    sprRnd.sprite = m_defaultSprite;
                }
            }
        }
	}
    protected override void BeginInteract()
    { 
        if (!m_interacted && m_triggerable)
        {
            m_triggerable.Trigger();
            m_interacted = true;
            m_timer.Restart();
            if (m_activeSprite)
            {
                SpriteRenderer sprRnd = GetComponent<SpriteRenderer>();
                if (sprRnd)
                {
                    sprRnd.sprite = m_activeSprite;
                }
            }
        }
    }

}
