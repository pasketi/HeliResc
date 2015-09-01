using UnityEngine;
using System.Collections;
using System;

public class HookableObject : MonoBehaviour, IHookable {

    protected Action UpdateMethod = () => { };      //Event to update all necessary methods depending on the type of the object

    public bool saveable = true;                    //Can the object be hooked
    public bool useTimer;
    public float timeToLive = 60;
    public Vector2 anchorWhenHooked;                //Where the hingejoint anchor should be
    public Vector2 connectedAnchor;                 //how far from the hook the object should be
    public int saveValue;                           //How much money the player should get from saving the item
    public int size = 1;                            //How much room in the cargo space the item will take
    public float mass = 1;


    protected float timer;
    protected Copter copter;                        //Reference to the player copter
    //protected Rigidbody2D hookRb;
    protected bool hooked;
    protected HingeJoint2D joint;                   //Reference to own distance joint
    protected HookScript hookScript;
    protected FloatingObject floating;

//    protected virtual void OnEnable() {
//        EventManager.StartListening("HookDied", DetachHook);
//    }
//    protected virtual void OnDisable() {
//        EventManager.StopListening("HookDied", DetachHook);
//    }

    protected virtual void Start() {
        
        copter = GameObject.Find("Copter").GetComponent<Copter>();
        hookScript = copter.HookScript;
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
            SaveItem();
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
        if (saveable == true)
            manager.saveCrates(1);
		hookScript.DetachHook (this);
        gameObject.SetActive(false);
    }
    public virtual void GrabHook(Rigidbody2D hookRb) {
        hooked = true;

        gameObject.layer = LayerMask.NameToLayer("liftedCrate");
        
        joint.enabled = true;
        
        hookScript.GrabHook(this);

        joint.connectedBody = hookRb;
        joint.connectedAnchor = connectedAnchor;
        joint.anchor = anchorWhenHooked;

        floating.enabled = false;
    }
    public virtual void DetachHook() {
		if (hooked == false)
			return;
        hooked = false;
        joint.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Crate");
        floating.enabled = true;
        hookScript.DetachHook(this);
                
    }
}

public interface IHookable {
    void GrabHook(Rigidbody2D hookRb);
    void DetachHook();
}