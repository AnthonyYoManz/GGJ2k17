using UnityEngine;
using System.Collections;

public class InteractableScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Interact()
    {
        BeginInteract();
    }

    protected virtual void BeginInteract()
    {
        //override and do stuff
    }
}
