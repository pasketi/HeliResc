using UnityEngine;
using System.Collections;

public class Shark : HookableObject {

    public float patrolRange;               //The amount of units the shark will move. Shark will start from middle Total range is 2 times the value of patrolrange
    public float speed;
    public Collider2D collider;

    protected Transform _transform;
    protected Rigidbody2D _rigidbody;
    protected bool movingRight;
    protected float leftLimit;
    protected float rightLimit;             //the most right x position the shark will patrol
    protected Vector3 sharkScale;    

    protected override void Start() {
        base.Start();

        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        sharkScale = _transform.localScale;
        collider.enabled = false;


        leftLimit = _transform.position.x - patrolRange;
        rightLimit = _transform.position.x + patrolRange;

        movingRight = transform.localScale.x < 0;

        _rigidbody.velocity = movingRight ? Vector2.right : -Vector2.right;
        _rigidbody.velocity *= speed;

        UpdateMethod += Patrolling;
    }

    public override void GrabHook(Rigidbody2D hookRb) {
        UpdateMethod -= Patrolling;
        collider.enabled = true;
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        base.GrabHook(hookRb);
    }
    public override void DetachHook() {
        UpdateMethod += Patrolling;
        collider.enabled = false;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        base.DetachHook();
    }

    protected void Patrolling() {
        if (_transform.position.x > rightLimit && movingRight == true) {
            movingRight = false;
            _rigidbody.velocity = Vector2.right * -speed;
            sharkScale.x *= -1;
            _transform.localScale = sharkScale;

        } 
        else if (_transform.position.x < leftLimit && movingRight == false) {
            movingRight = true;
            _rigidbody.velocity = Vector2.right * speed;
            sharkScale.x *= -1;
            _transform.localScale = sharkScale;
        }
    }
}
