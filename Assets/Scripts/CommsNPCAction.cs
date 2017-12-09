using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommsNPCAction : MonoBehaviour, INPCAction {

    private Animator anim;

    public string npcName()
    {
        return "Comms";
    }

    public void setActive()
    {
        throw new NotImplementedException();
    }

    public void setDeactive()
    {
        throw new NotImplementedException();
    }

    public void talk()
    {
        anim.SetTrigger("Talk");
    }

    public void idle()
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake()
    {
        anim = transform.GetComponent<Animator>();
    }
}
