﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager   s_singleton;
    private Player[]            m_players;
    private CameraScript        m_cameraScript;
    private Vector2[]           m_playerPositions;
	// Use this for initialization
	void Awake ()
    {
        Debug.Assert(s_singleton == null, "Only one game manager allowed");
        s_singleton = this;
        DontDestroyOnLoad(this.gameObject);

    }

    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        m_players = new Player[players.Length];
        for (int i = 0; i < players.Length; ++i) m_players[i] = players[i].GetComponent<Player>();

        m_cameraScript = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraScript>();
        //change room changing to go through this script 
    }

    // Update is called once per frame
    void Update ()
    {
        foreach (Player p in m_players)
        {
            //if (p.m_curState == Player.STATE.DEAD)
            {
                //if input == restart
                {
                    RestartLevel();
                }
            }
        }

	}


    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        int idx = 0;
        foreach (Player p in m_players)
        {
            p.transform.position = m_playerPositions[++idx];
        }
    }
}