﻿using UnityEngine;
using System.Collections;

public class WinPadScript : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GameManager.s_singleton.Win();
        }
    }

}
