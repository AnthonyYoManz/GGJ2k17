using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomScript : MonoBehaviour
{

    public Rect m_size { get; private set;}


    public BoxCollider2D m_collider { get; private set; }

    //Don't know if we need any update stuff yet so leave it for now
	// Use this for initialization
	void Awake ()
    {
        m_collider = GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
