using UnityEngine;
using System.Collections;

public class VerticalSineScript : MonoBehaviour
{

    public float m_magnitude = 1.0f;
    public float m_speed = 1.0f;
    private float m_curTime;
    private Vector3 m_startPos;

	// Use this for initialization
	void Awake ()
    {
        m_curTime = 0;
        m_startPos = transform.position;
        Vector3 newPos = transform.position;
        newPos.y = m_startPos.y + Mathf.Sin(m_curTime) * m_magnitude;
        transform.position = newPos;
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_curTime += Time.deltaTime * m_speed * Mathf.PI;
        if (m_curTime > 2 * Mathf.PI)
        {
            m_curTime -= 2 * Mathf.PI;
        }
        Vector3 newPos = transform.position;
        newPos.y = m_startPos.y + Mathf.Sin(m_curTime) * m_magnitude;
        transform.position = newPos;
	}
}
