using UnityEngine;
using System.Collections;

public class LeverScript : InteractableScript {

    public TriggerableScript m_triggerable; //make this do thing thx
    public Sprite m_activeSprite;

    private bool m_interacted;

	// Use this for initialization
	void Start ()
    {
        m_interacted = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    protected override void BeginInteract()
    {
        if(!m_interacted && m_triggerable)
        {
            m_triggerable.Trigger();
            m_interacted = true;
            if(m_activeSprite)
            {
                SpriteRenderer sprRnd = GetComponent<SpriteRenderer>();
                if(sprRnd)
                {
                    sprRnd.sprite = m_activeSprite;
                }
            }
        }
    }

    public override bool IsInteractable()
    {
        return !m_interacted;
    }
}
