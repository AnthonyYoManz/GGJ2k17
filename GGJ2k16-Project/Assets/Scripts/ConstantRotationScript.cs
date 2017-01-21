using UnityEngine;
using System.Collections;

public class ConstantRotationScript : MonoBehaviour {

    public float m_rotationSpeed = 100.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, m_rotationSpeed * Time.deltaTime);
	}
}
