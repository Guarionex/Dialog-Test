using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienNPCAction : MonoBehaviour, INPCAction {
    
    [SerializeField]
    private Transform hologram;
    private Animator anim;
    private Vector3 hologramPosition;
    private Vector3 hiddenHologramPosition;

    public void setActive()
    {
        transform.gameObject.SetActive(true);
        hologram.position = hologramPosition;
    }

    public void setDeactive()
    {
        transform.gameObject.SetActive(false);
        hologram.position = hiddenHologramPosition;
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
        
        hologramPosition = new Vector3(0.7f, 1.4f, -1.4f);
        hiddenHologramPosition = new Vector3(0.7f, 9.0f, -1.4f);
    }
}
