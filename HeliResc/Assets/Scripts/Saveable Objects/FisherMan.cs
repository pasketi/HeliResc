using UnityEngine;
using System.Collections;

public class FisherMan : SaveableObject {

    public GameObject legs;
    public Animator animator;

    protected LevelManager manager;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        manager = GameObject.FindObjectOfType<LevelManager>();
        legs.SetActive(false);
	}

    protected override void OnTriggerEnter2D(Collider2D other) {
        if(other.tag.Equals("Hook") && hooked == false && other.gameObject != legs) {
            
            GameObject.FindObjectOfType<HookScript>().GrabHook(this);
            hookRb = other.GetComponent<Rigidbody2D>();

            GrabHook();
            
        }
    }

    public override void GrabHook() {
        hooked = true;

        gameObject.layer = LayerMask.NameToLayer("liftedCrate");

        joint.enabled = true;        

        joint.connectedBody = hookRb;
        joint.connectedAnchor = connectedAnchor;
        joint.anchor = anchorWhenHooked;

        legs.SetActive(true);
    }

    public override void DetachHook() {
        base.DetachHook();
    }
}
