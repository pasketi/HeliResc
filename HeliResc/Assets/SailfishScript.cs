using UnityEngine;
using System.Collections;

public class SailfishScript : MonoBehaviour {

    public float jumpForce;

    private Copter copter;
    private Rigidbody2D _rigidbody;
    private Transform _transform;

    private Vector2 forceVector;
    private float startPosition;

    void Start() {
        copter = GameObject.Find("Copter").GetComponent<Copter>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = transform;
        forceVector = new Vector2(0, jumpForce);


        startPosition = _transform.position.y;
    }

    void Update() {
        Vector3 rotation = new Vector3(0, 0, -90);
        rotation.z *= Mathf.Sign(_rigidbody.velocity.y);

        _transform.rotation = Quaternion.Euler(rotation);

        if (_transform.position.y < startPosition)
            Jump();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        string tag = collision.gameObject.tag;
        if (tag.Equals("Copter")) {
            copter.Detonate();
        }
        else if (tag.Equals("Hook")) {
            copter.rope.KillHook();
        }
    }

    private void Jump() {
        _rigidbody.velocity = forceVector;
    }
}
