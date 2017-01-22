using UnityEngine;
using System.Collections;

public class TimedSuicideScript : MonoBehaviour {

    public float m_timeToDie = 2.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        m_timeToDie -= Time.deltaTime;
        if(m_timeToDie<=0)
        {
            Destroy(gameObject);
        }
	}
}
