using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour
{
    public string           m_playerTag = "Player";
    private List<Player>    m_players;
    // Use this for initialization
    private RoomScript      m_curRoom;
    private Camera          m_camera;
    private Vector2         m_cameraExtents;
   // private RoomScript m_prevRoom;need this?
    void Awake()
    {
        m_players = new List<Player>();
        m_camera = GetComponent<Camera>();
    }
	void Start ()
    {
        m_cameraExtents.x = m_camera.orthographicSize;
        m_cameraExtents.y = m_cameraExtents.x * Screen.width / Screen.height;

        GameObject[] players = GameObject.FindGameObjectsWithTag(m_playerTag);
        foreach (GameObject player in players)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript)
            {
                m_players.Add(playerScript);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        /*Move to average of player positions*/
        Vector2 position = Vector2.zero;
        foreach (var player in m_players)
        {
            position += (Vector2)player.transform.position;
        }
        position /= m_players.Count;

        Vector2 roomCenter = m_curRoom.m_collider.bounds.center;
       
        if (position.y + m_cameraExtents.y >= roomCenter.y + m_curRoom.m_collider.bounds.extents.y)
        {
            position.y = roomCenter.y + m_curRoom.m_collider.bounds.extents.y;
        }
        else if (position.y - m_cameraExtents.y <= roomCenter.y - m_curRoom.m_collider.bounds.extents.y)
        {
            position.y = roomCenter.y - m_curRoom.m_collider.bounds.extents.y;
        }

        if (position.x + m_cameraExtents.x >= roomCenter.x + m_curRoom.m_collider.bounds.extents.x)
        {
            position.x = roomCenter.x + m_curRoom.m_collider.bounds.extents.x;
        }
        else if (position.x - m_cameraExtents.x <= roomCenter.x - m_curRoom.m_collider.bounds.extents.x)
        {
            position.x = roomCenter.x - m_curRoom.m_collider.bounds.extents.x;
        }
    }

    public void MoveToRoom(RoomScript _room)
    {

    }
}
