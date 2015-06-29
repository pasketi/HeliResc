using UnityEngine;
using System.Collections;
using System;

public class SaveableObject : MonoBehaviour, IHookable {

    protected Action UpdateMethod = () => { };      //Event to update all necessary methods depending on the type of the object

    public bool canGrabHook;                        //Can the object be hooked
    public bool useTimer;
    public float timeToLive = 60;

    protected Copter copter;                        //Reference to the player copter
    protected Rigidbody2D hookRb;
    protected bool hooked;
    protected HingeJoint2D joint;                   //Reference to own distance joint

    protected virtual void Start() {

        copter = GameObject.Find("Copter").GetComponent<Copter>();
        joint = GetComponent<HingeJoint2D>();
        joint.enabled = false;

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
    public virtual void GrabHook() {
        hooked = true;

        GetComponent<Collider2D>().isTrigger = false;
        GetComponent<FloatingObject>().enabled = false;
        joint.enabled = true;

        hookRb.GetComponent<HookScript>().GrabHook(this);

        joint.connectedBody = hookRb;
        joint.connectedAnchor = new Vector2(0f, -0.1f);
        joint.anchor = new Vector2(0f, 1);
    }
    public virtual void DetachHook() {
        hooked = false;
        joint.enabled = false;
        GetComponent<Collider2D>().isTrigger = true;
        GetComponent<FloatingObject>().enabled = true;
    }
}

public interface IHookable {
    void GrabHook();
    void DetachHook();
}