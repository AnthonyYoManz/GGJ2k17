using UnityEngine;
using System.Collections;

public class DoorScript : TriggerableScript
{

    private Vector3 m_startPos;
    private Vector3 m_targetPos;
    private float m_timeSinceTrigger;
    public float m_timeToOpen = 0.3f;
    public Vector2 m_openOffset;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if(m_triggered)
        {
            m_timeSinceTrigger += Time.deltaTime;
            float progress = m_timeSinceTrigger / m_timeToOpen;
            Vector3 newPos = Vector3.Lerp(m_startPos, m_targetPos, progress);
            transform.position = newPos;
        }
	}

    protected override void OnTrigger()
    {
        m_startPos = transform.position;
        m_targetPos = m_startPos + (Vector3)m_openOffset;
        m_timeSinceTrigger = 0.0f;
    }
}
