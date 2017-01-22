using UnityEngine;
using System.Collections;

public class WinPadScript : InteractableScript
{
    public float m_scaleDuratoin;

    bool m_interacted;
    private Timer m_scaleTimer;
    Animator m_anim;

    //private Vector3 startScale
    // Use this for initialization
    void Awake()
    {
        m_scaleTimer = new Timer();
        m_anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
     
    }


    protected override void BeginInteract()
    {
        m_interacted = true;
        GameManager.s_singleton.Win();
        m_anim.SetTrigger("dw");
        //m_scaleTimer.Restart();
    }

}