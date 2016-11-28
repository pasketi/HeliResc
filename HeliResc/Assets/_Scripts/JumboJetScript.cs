using UnityEngine;
using System.Collections;

public class JumboJetScript : MonoBehaviour {

    public float maxHeight;
    public float speed = 40;

    private Transform copterTransform;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private SpriteRenderer _sprite;
    private bool inAction;

    void Awake() {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();

        _collider.enabled = false;
        _sprite.enabled = false;
        _rigidbody.gravityScale = 0;

        maxHeight = transform.position.y - 3.025f;
    }

	// Use this for initialization
	void Start () {		
        copterTransform = GameObject.Find("Copter").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (inAction == false && copterTransform.position.y >= maxHeight) {
            inAction = true;
            _sprite.enabled = true;
            _collider.enabled = true;
            _transform.position = copterTransform.position + Vector3.right * 20;
            _rigidbody.velocity = -Vector3.right * speed;

            Invoke("Reset", 3);
        }        
	}    

    void OnTriggerEnter2D(Collider2D other) {
        Copter copter = other.GetComponent<Copter>();
        if (copter != null)
            copter.Detonate();
    }

    private void Reset() {
        inAction = false;
    }
}
