using UnityEngine;
using System.Collections;

public class AirBalloon : MonoBehaviour {

    public Sprite happySprite;
    public Sprite scaredSprite;
    public float scaredVelocity;
    public float scaredAngle = 5;


    private SpriteRenderer _sprite;
    private Transform _transform;
    private Rigidbody2D _rigidbody;

    void Awake() {
        HingeJoint2D hinge = GetComponent<HingeJoint2D>();
        hinge.connectedAnchor = ((Vector2)transform.position) + hinge.anchor;

        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    void Update() {
        float angle = _transform.eulerAngles.z;
        if (angle > 180)
            angle = 360 - angle;
        if (angle > scaredAngle || Mathf.Abs(_rigidbody.angularVelocity) > scaredVelocity) {
            _sprite.sprite = scaredSprite;
        }
        else {
            _sprite.sprite = happySprite;
        }
    }
}
