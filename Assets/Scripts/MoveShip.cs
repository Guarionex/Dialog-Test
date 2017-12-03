using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShip : MonoBehaviour {
    
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Vector3 _direction;
    private float _duration;
    private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        
    }

    void FixedUpdate ()
    {
        
        if (_duration > 0)
        {
            Vector3 speedVector = _direction * _speed;
            Vector3 moveVector = speedVector * Time.deltaTime;
            transform.position += moveVector;
            _duration -= Time.deltaTime;
        }
        Quaternion currentRotation = transform.rotation;
        Quaternion wantedRotation = Quaternion.Euler(_direction);
        transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * _speed);
    }

    public void moveBySpeedDirection(float speed, float duration, Vector3 direction)
    {
        _speed = speed;
        _direction = direction;
        _duration = duration;
    }

    public void stopMoving()
    {
        _speed = 0;
    }

}
