using UnityEngine;
using System.Collections;
using System;

public class SaveableObject : MonoBehaviour, IHookable {

    protected Action UpdateMethod = () => { };      //Event to update all necessary methods depending on the type of the object

    public bool canGrabHook;                        //Can the object be hooked
    public bool useTimer;
    public float timeToLive = 60;
    public Vector2 anchorWhenHooked;                //Where the hingejoint anchor should be
    public Vector2 connectedAnchor;                 //how far from the hook the object should be
    public int saveValue;                           //How much money the player should get from saving the item


    protected float timer;
    protected Copter copter;                        //Reference to the player copter
    //protected Rigidbody2D hookRb;
    protected bool hooked;
    protected HingeJoint2D joint;                   //Reference to own distance joint
    protected HookScript hookScript;
    protected FloatingObject floating;

    protected virtual void Start() {
        
        copter = GameObject.Find("Copter").GetComponent<Copter>();
        joint = GetComponent<HingeJoint2D>();
        floating = GetComponent<FloatingObject>();
        joint.enabled = false;

        timer = timeToLive;

        if (useTimer == true) UpdateMethod += Timer;        
    }

    protected virtual void Update() {
        UpdateMethod();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("LandingPad")) {
            
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Hook") && hooked == false) {            
            GrabHook(other.GetComponent<Rigidbody2D>());
        }
    }

    protected virtual void Timer() {
        timer -= Time.deltaTime;
    }
    public virtual void CargoItem() {
        hookScript.DetachHook(this);
        gameObject.SetActive(false);
    }
    public virtual void SaveItem() {
        LevelManager manager = GameObject.FindObjectOfType<LevelManager>();
        manager.saveCrates(1);
        gameObject.SetActive(false);
    }
    public virtual void GrabHook(Rigidbody2D hookRb) {
        hooked = true;

        gameObject.layer = LayerMask.NameToLayer("liftedCrate");
        
        joint.enabled = true;

        hookScript = hookRb.GetComponent<HookScript>();
        hookScript.GrabHook(this);

        joint.connectedBody = hookRb;
        joint.connectedAnchor = connectedAnchor;
        joint.anchor = anchorWhenHooked;

        floating.enabled = false;
    }
    public virtual void DetachHook() {
        hooked = false;
        joint.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Crate");
        floating.enabled = true;
                
    }
}

public interface IHookable {
    void GrabHook(Rigidbody2D hookRb);
    void DetachHook();
}