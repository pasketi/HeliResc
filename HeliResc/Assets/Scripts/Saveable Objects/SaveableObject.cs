using UnityEngine;
using System.Collections;
using System;

public class SaveableObject : MonoBehaviour {

    protected Action UpdateMethod = () => { };      //Event to update all necessary methods depending on the type of the object

    public bool canGrabHook;                        //Can the object be hooked
    public bool useTimer;
    public float timeToLive = 60;

    protected Copter copter;                        //Reference to the player copter
    protected Rigidbody2D hookRb;
    protected bool hooked;
    protected HingeJoint2D joint;                //Reference to own distance joint

    protected virtual void Start() {

        copter = GameObject.Find("Copter").GetComponent<Copter>();
        joint = GetComponent<HingeJoint2D>();

        if (useTimer == true) UpdateMethod += Timer;
    }

    protected virtual void Update() {
        UpdateMethod();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("LandingPad")) {
            SaveItem();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Hook") && hooked == false) {
            hookRb = other.GetComponent<Rigidbody2D>();
            GrabHook();
        }
    }

    protected virtual void Timer() {
        timeToLive -= Time.deltaTime;
    }
    protected virtual void SaveItem() { 
    
    }
    protected virtual void GrabHook() {
        hooked = true;
        GetComponent<Rigidbody2D>().mass = 0;
        //GetComponent<FloatingObject>().enabled = false;
        joint.enabled = true;        
        transform.parent = hookRb.transform;
        joint.connectedBody = hookRb;
        joint.connectedAnchor = new Vector2(0f, -0.3f);
        joint.anchor = new Vector2(0f, 1);
    }
    protected virtual void DetachHook() {
        hooked = false;
        joint.enabled = false;
        transform.parent = null;
    }
}
