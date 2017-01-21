using UnityEngine;
using System.Collections;

public class TriggerableScript : MonoBehaviour {

    [SerializeField]
    protected bool m_triggered;

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
            m_triggered = true;
            OnTrigger();
        }
    }

    protected virtual void OnTrigger()
    {
        //override and do stuff
    }
}
