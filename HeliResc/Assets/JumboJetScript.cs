using UnityEngine;
using System.Collections;

public class JumboJetScript : MonoBehaviour {

    public Transform activationHeight;

    private Transform copterTransform;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private bool inAction;

	void Awake() {
		Debug.Log ("Awake position: " + transform.position);
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("Start position 1: " + transform.position);
		_transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        copterTransform = GameObject.Find("Copter").transform;
		Debug.Log ("Start position 2: " + _transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Position: " + _transform.position);
        if (copterTransform.position.y > activationHeight.position.y && inAction == false) {
            inAction = true;
			Debug.Log ("Activate");
            _transform.position = copterTransform.position + Vector3.right * 20 - activationHeight.localPosition;
            _rigidbody.velocity = -Vector3.right * 20;
        }
        if (inAction == true) { 
        
        }
	}
    void OnTriggerEnter2D(Collider2D other) {
        Copter copter = other.GetComponent<Copter>();
        if (copter != null)
            copter.Detonate();
    }
}
