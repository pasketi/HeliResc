using UnityEngine;
using System;
using System.Collections;

public class TucanScript : MonoBehaviour {

    public float speed;
    public float patrolRange;

    private Action Patrol = () => { };

    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private Vector3 target;
    private Vector3 scale;
    private Vector3 rightTarget;
    private Vector3 leftTarget;

	// Use this for initialization
	void Start () {

        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();

        rightTarget = _transform.position + new Vector3(patrolRange, 0);
        leftTarget = _transform.position - new Vector3(patrolRange, 0);        
        target = new Vector3();
        scale = _transform.localScale;
        Patrol = MoveRight;

	}
	
	// Update is called once per frame
	void Update () {
        Patrol();
        //Rotation();
	}
    private void Rotation() {
        float angle = Vector3.Angle((target-_transform.position).normalized, Vector3.right * _transform.localScale.x);
        Debug.Log("Angle" + angle);
        Vector3 newAngle = new Vector3(0, 0, angle);
        _transform.eulerAngles = newAngle;
    }
    private void ChangeDirection(bool goRight) {
        if (goRight == true) {
            target = rightTarget;                        
            Patrol = MoveRight;
        }
        else {
            target = leftTarget;
            Patrol = MoveLeft;
        }
        scale.x *= -1;
        _transform.localScale = scale;
    }

    private void MoveLeft() {
        Vector3 direction = (leftTarget - _transform.position).normalized;
        _rigidbody.AddForce(direction * speed * Time.deltaTime);
        if (_transform.position.x < leftTarget.x || Vector3.SqrMagnitude(direction) < 0.25f) {
            ChangeDirection(true);
        }
    }
    private void MoveRight() {
        Vector3 direction = (rightTarget - _transform.position).normalized;
        _rigidbody.AddForce(direction * speed * Time.deltaTime);
        if (_transform.position.x > rightTarget.x || Vector3.SqrMagnitude(direction) < 0.25f) {
            ChangeDirection(false);
        }
    }    
}
