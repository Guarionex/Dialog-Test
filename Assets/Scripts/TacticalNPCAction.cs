using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalNPCAction : MonoBehaviour, INPCAction {

    private Animator anim;
    [SerializeField]
    private Transform talkingTarget;
    [SerializeField]
    private float rotationSpeed;
    private Quaternion idleRotation;
    private bool isTalking;
    private bool isFacingTarget;
    private bool isReturningtoIdle;

    public string npcName()
    {
        return "Tactical";
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
        if (isFacingTarget)
        {
            anim.SetTrigger("Talk");
        }
        else
        {
            anim.SetTrigger("Walk");
        }
        isTalking = true;
        anim.SetBool("Pure_Idle", true);
    }

    public void idle()
    {
        isTalking = false;
        isReturningtoIdle = true;
        if (isInIdleRotation())
        {
            anim.SetBool("Pure_Idle", false);
        }
        else
        {
            anim.SetTrigger("Walk");
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isTalking && !isFacingTarget)
        {
            faceToTalk();
        }
        else if (!isTalking && !isInIdleRotation() && isReturningtoIdle)
        {
            rotateToIdle();
        }
    }

    void Awake()
    {
        anim = transform.GetComponent<Animator>();
        idleRotation = transform.rotation;
        isTalking = false;
        isFacingTarget = false;
        isReturningtoIdle = false;
    }

    private void faceToTalk()
    {
        anim.SetBool("Moving", true);
        Vector3 targetDir = talkingTarget.position - transform.position;
        float step = rotationSpeed * Time.deltaTime;
        Vector3 prevForward = transform.forward;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
        if (prevForward == newDir)
        {
            isFacingTarget = true;
            anim.SetBool("Moving", false);
            anim.SetTrigger("Talk");
        }
    }

    private void rotateToIdle()
    {
        Debug.Log("Tactics rotating");
        anim.SetBool("Moving", true);
        isFacingTarget = false;
        float step = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, idleRotation, step * Mathf.Rad2Deg);
        if (isInIdleRotation())
        {
            isReturningtoIdle = false;
            anim.SetBool("Moving", false);
            anim.SetBool("Pure_Idle", false);
        }
    }

    private bool isInIdleRotation()
    {
        return transform.rotation == idleRotation;
    }
}
