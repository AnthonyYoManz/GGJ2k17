﻿using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour
{
    public CameraScript m_camera;
    public RoomScript   m_firstRoom;
    public RoomScript   m_secondRoom;
    public bool         m_victoryExit;
    public bool         m_allowRoomMove;
    //public bool         m_oneWay;
    public DoorScript   m_door;
    public Transform[]  m_roomStartPositions;
    public Transform    m_offscreenEntry;
    public bool         m_intro;
    private bool        m_snappedToEntry;
    private Collider2D  m_collider;
    public enum TYPE { NORMAL, QUIT, WIN }
    public TYPE         m_doorType;
    void Awake()
    {
        m_collider = GetComponent<Collider2D>();
        
    }
	// Use this for initialization
	void Start ()
    {
        m_intro = false;
        playerInPos = new bool[GameManager.s_singleton.GetPlayers().Length];
    }


    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireCube(m_camera.transform.position, m_camera.m_cameraExtents*2);
    //}

    // Update is called once per frame
    bool[] playerInPos;
    void Update ()
    {
        if (m_intro)
        {
            int idx = 0;
            int playersInPos = 0;
            
            foreach (Player p in GameManager.s_singleton.GetPlayers())
            { 
                if (p.MoveTowards(m_roomStartPositions[idx++].position, p.GetMaxMoveSpeed() * Time.deltaTime))
                {
                    playersInPos++;
                    p.ForceAnimation(Player.ANIM_STATE.IDLE);
                    p.ForceSetAnimSpeed(0.0f);
                }

            }
            if (m_camera.AtAnchorPosition() && playersInPos == GameManager.s_singleton.GetPlayers().Length)
            {
                EndWalkThrough();
            }
            else if (!m_snappedToEntry && m_camera.AtAnchorPosition())
            {
                Vector2 temp = m_camera.transform.position;
                Rect camRect = new Rect(temp - m_camera.m_cameraExtents, m_camera.m_cameraExtents * 2);
                foreach (Player p in GameManager.s_singleton.GetPlayers())
                {
                    if (!camRect.Contains(p.transform.position))
                    {
                        p.transform.position = m_offscreenEntry.position;
                    }
                }
                m_snappedToEntry = true;
            }
        }
	}
    void StartWalkThrough()
    {
        m_collider.enabled = false;
        m_camera.MoveToRoom(m_secondRoom);
        GameManager.s_singleton.AllowPlayerInput(false);
        m_intro = true;
        m_snappedToEntry = false;
        if (m_secondRoom != null) m_secondRoom.PlayersAreReady();
    }

    void EndWalkThrough()
    {
        m_snappedToEntry = false;
        m_collider.enabled = true;
        m_intro = false;
        m_collider.enabled = true;
        GameManager.s_singleton.AllowPlayerInput(true);
        if (m_door != null) m_door.CloseDoor();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            switch (m_doorType)
            {
                case TYPE.WIN: GameManager.s_singleton.Win(); break;
                case TYPE.QUIT: GameManager.s_singleton.QuitGame(); break;
                case TYPE.NORMAL:
                {
                    if (m_allowRoomMove)
                    {
                        if (m_firstRoom != null && m_camera.m_curRoom == m_firstRoom)
                        {
                            StartWalkThrough();

                        }
                        //else if (m_secondRoom != null && m_camera.m_curRoom == m_secondRoom && !m_oneWay)
                        //{
                        //    StartWalkThrough();
                        //}
                    }
                    break;
                }
            }
        }
    }
}
