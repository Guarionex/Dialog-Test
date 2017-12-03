using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondOfficerNPCAction : MonoBehaviour, INPCAction {

    private Animator anim;

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

    public string npcName()
    {
        return "XO";
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
