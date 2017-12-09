using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienNPCAction : MonoBehaviour, INPCAction {

    private Animator anim;

    public void setActive()
    {
        transform.gameObject.SetActive(true);
    }

    public void setDeactive()
    {
        transform.gameObject.SetActive(false);
    }

    public void talk()
    {

        setActive();
        anim.SetTrigger("Talk");
    }

    public string npcName()
    {
        return "Alien";
    }

    // Use this for initialization
    void Start () {
        
    }

    public void idle()
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void Awake()
    {
        anim = transform.GetComponent<Animator>();
    }
}
