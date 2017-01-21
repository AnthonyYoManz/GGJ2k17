using UnityEngine;
using System.Collections;

public class LeverScript : InteractableScript {

    public TriggerableScript m_triggerable; //make this do thing thx

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
        if(!m_interacted)
        {
            m_triggerable.Trigger();
            m_interacted = true;
        }
    }
}
