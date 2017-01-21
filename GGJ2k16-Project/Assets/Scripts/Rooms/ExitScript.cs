using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour
{
    public CameraScript m_camera;
    public RoomScript   m_firstRoom;
    public RoomScript   m_secondRoom;
    public bool         m_oneWay;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (m_firstRoom !=null && m_camera.m_curRoom == m_firstRoom)
            {
                m_camera.MoveToRoom(m_secondRoom);
            }
            else if (m_secondRoom != null && m_camera.m_curRoom == m_secondRoom && !m_oneWay)
            {
                m_camera.MoveToRoom(m_firstRoom);
            }
        }
    }
}
