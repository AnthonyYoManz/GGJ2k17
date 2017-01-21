using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    
    public float m_duration;
    private float m_time;
    public Image m_image;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_time += Time.deltaTime;
        m_image.color = Color.Lerp(Color.black, Color.clear, m_time/ m_duration);
    }
}
