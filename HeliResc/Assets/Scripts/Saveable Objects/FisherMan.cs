using UnityEngine;
using System.Collections;

public class FisherMan : HookableObject, IChainable {

    public GameObject legs;
    public Animator animator;
    public Transform skullTransform;
    public Vector2 chainAnchor;
    public Vector2 chainConnectedAnchor;
    [HideInInspector]
    public bool HasDude;

    protected string waterString = "inWater";
    protected string hookedString = "hooked";
    protected string timerString = "timer";
    protected string dudeString = "hasDude";    

    protected Rigidbody2D _rigidbody;
    protected Transform hookedTransform;
    protected bool dead;
    protected LevelManager manager;

	// Use this for initialization
	protected override void Start () {
        base.Start();        

        manager = GameObject.FindObjectOfType<LevelManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        
        legs.SetActive(false);                

        UpdateMethod += UpdateAnimator;
	}
	
    protected override void OnTriggerEnter2D(Collider2D other) {
        if(other.tag.Equals("Hook") && hooked == false && other.gameObject != legs) {
            
            HookScript hs = GameObject.FindObjectOfType<HookScript>();
            if (hs == null) return;     //Check if the hook is out

            hs.GrabHook(this);
            
            hookedTransform = other.transform;
            animator.Play("Hanging");            
            
            
            GrabHook(other.GetComponent<Rigidbody2D>());            
            
        }
    }   

    protected virtual void UpdateAnimator() {
        animator.SetBool(waterString, floating.IsInWater);
        animator.SetBool(hookedString, hooked);
        animator.SetBool(dudeString, HasDude);
        animator.SetFloat(timerString, timer);
    }

    protected virtual void UpdateSkull() {
        skullTransform.localPosition = Vector3.up * 1.5f;
    }

    protected override void Timer() {
        base.Timer();                    
        if (timer <= 0) {
            Die();
        }
    }

    protected void Die() {
        animator.Play("Dead");
		dead = true;
        UpdateMethod = () => { };
        UpdateMethod += UpdateSkull;
        skullTransform.gameObject.SetActive(true);
    }

    public override void GrabHook(Rigidbody2D hookRb) {
        if (dead == true) return;
		if (useTimer == true) UpdateMethod -= Timer;
        hooked = true;                

        gameObject.layer = LayerMask.NameToLayer("liftedCrate");

        joint.enabled = true;        

        joint.connectedBody = hookRb;

        if (hookRb.name.Equals("Legs")) {
            hookRb.transform.root.GetComponent<FisherMan>().HasDude = true;
            joint.connectedAnchor = chainConnectedAnchor;
            joint.anchor = chainAnchor;
        }
        else {
            joint.connectedAnchor = connectedAnchor;
            joint.anchor = anchorWhenHooked;
        }
        

        legs.SetActive(true);

        _rigidbody.constraints = RigidbodyConstraints2D.None;
        floating.enabled = false;
    }

    public override void DetachHook() {
		if (hooked == false) return;
        base.DetachHook();

		if (useTimer == true) UpdateMethod += Timer;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        floating.enabled = true;

        FisherMan fm = hookedTransform.root.GetComponent<FisherMan>();
        if (fm != null) fm.HasDude = false;
    }
    public void Chain(Rigidbody2D rb) {
        GrabHook(rb);
    }
}
public interface IChainable {
    void Chain(Rigidbody2D rb);
}
