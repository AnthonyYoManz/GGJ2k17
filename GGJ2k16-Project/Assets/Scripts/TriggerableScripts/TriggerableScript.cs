using UnityEngine;
using System.Collections;
using System;

public class TriggerableScript : MonoBehaviour {

    [SerializeField]
    protected bool m_triggered;
    public int m_triggers = 1;

    // Use this for initialization
    void Start ()
    {
        m_triggered = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void Trigger()
    {
        if (!m_triggered)
        {
            m_triggers--;
            if (m_triggers <= 0)
            {
                m_triggered = true;
                OnTrigger();
            }
        }
    }

    internal void UndoTrigger()
    {
        if (!m_triggered)
        {
            m_triggers++;
        }
    }

    protected virtual void OnTrigger()
    {
        //override and do stuff
    }

    public bool Triggered()
    {
        return m_triggered;
    }
}
