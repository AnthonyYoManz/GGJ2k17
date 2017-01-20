﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour
{
    public string           m_playerTag = "Player";
    public float            m_speed;
    private List<Player>    m_players;
    // Use this for initialization
    public RoomScript       m_curRoom;
    public Camera          m_camera;
    private Vector2         m_cameraExtents;
    private Vector3         m_target;
    private int             m_camZ = -10;
   // private RoomScript m_prevRoom;need this?
    void Awake()
    {
        m_players = new List<Player>();
        //m_camera = GetComponent<Camera>();
    }
	void Start ()
    {
        m_cameraExtents.y = m_camera.orthographicSize;
        m_cameraExtents.x = m_cameraExtents.y * 16/9;

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
        Vector3 temp = transform.position;
        temp.z = m_camZ;
        transform.position = temp;


        /*Move to average of player positions*/
        Vector2 position = Vector2.zero;
        foreach (var player in m_players)
        {
            position += (Vector2)player.transform.position;
        }
        position /= m_players.Count;

        Vector2 roomCenter = m_curRoom.transform.position;
       
        if (position.y + m_cameraExtents.y >= roomCenter.y + m_curRoom.m_collider.bounds.extents.y)
        {
            position.y = roomCenter.y + m_curRoom.m_collider.bounds.extents.y - m_cameraExtents.y;
        }
        else if (position.y - m_cameraExtents.y <= roomCenter.y - m_curRoom.m_collider.bounds.extents.y)
        {
            position.y = roomCenter.y - m_curRoom.m_collider.bounds.extents.y + m_cameraExtents.y;
        }

        if (position.x + m_cameraExtents.x >= roomCenter.x + m_curRoom.m_collider.bounds.extents.x)
        {
            position.x = roomCenter.x + m_curRoom.m_collider.bounds.extents.x - m_cameraExtents.x;
        }
        else if (position.x - m_cameraExtents.x <= roomCenter.x - m_curRoom.m_collider.bounds.extents.x)
        {
            position.x = roomCenter.x - m_curRoom.m_collider.bounds.extents.x + m_cameraExtents.x;
        }

     

        m_target = new Vector3(position.x, position.y, m_camZ);
        Vector3 newpos = Vector3.Lerp(transform.position, m_target, m_speed*Time.deltaTime);
        transform.position = newpos;
    }

    public void MoveToRoom(RoomScript _room)
    {

    }
}
