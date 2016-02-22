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
    [HideInInspector]
    public bool grabLegs;
    [HideInInspector]
    public System.Collections.Generic.List<FisherMan> fishermenInLegs;

    protected string waterString = "inWater";
    protected string hookedString = "hooked";
    protected string timerString = "timer";
    protected string dudeString = "hasDude";    

    protected Rigidbody2D _rigidbody;
    protected Transform hookedTransform;
    protected bool dead;
    protected LevelManager manager;

//    protected override void OnEnable()
//    {
//        EventManager.StartListening("HookDied", DetachHook);
//    }
//    protected override void OnDisable()
//    {
//        EventManager.StopListening("HookDied", DetachHook);
//    }

	// Use this for initialization
	protected override void Start () {
        base.Start();

        fishermenInLegs = new System.Collections.Generic.List<FisherMan>();

        manager = GameObject.FindObjectOfType<LevelManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        
        legs.SetActive(false);                

        UpdateMethod += UpdateAnimator;
	}
	
    protected override void OnTriggerEnter2D(Collider2D other) {
		if (dead == true)
			return;
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
		if (floating.IsInWater == true) {
			floating.ChangeToStatic(true);
		}
    }

    protected override void Timer() {
        base.Timer();                    
        if (timer <= 0) {
            Die();
        }
    }
    public override void SaveItem() {
        if (fishermenInLegs != null) { 
            foreach(FisherMan f in fishermenInLegs)
                if(f != null) 
                    f.SaveItem(); 
        }
        base.SaveItem();
        Destroy(gameObject);
    }

    protected void Die() {
        animator.Play("Dead");
		dead = true;
        UpdateMethod = () => { };
        UpdateMethod += UpdateSkull;
        skullTransform.gameObject.SetActive(true);
		if (!manager.getResetButton()) manager.setResetButton(true);
    }

    public override void GrabHook(Rigidbody2D hookRb) {
        if (dead == true) return;
		if (useTimer == true) UpdateMethod -= Timer;
        hooked = true;                

        gameObject.layer = LayerMask.NameToLayer("liftedCrate");

        joint.enabled = true;        

        joint.connectedBody = hookRb;

        if (hookRb.name.Equals("Legs")) {
            FisherMan fm = hookRb.transform.parent.GetComponent<FisherMan>();
			if(fm.hooked == false) return;
            fm.HasDude = true;
            fm.fishermenInLegs.Add(this); 
            grabLegs = true;
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

    public override void DetachHook()
    {
        if (hooked == false || dead == true) return;

		GetComponent<HingeJoint2D> ().enabled = false;
        Debug.Log(gameObject.name);
        joint.connectedBody = null;

        hooked = false;
        grabLegs = false;
        HasDude = false;
        joint.enabled = false;
        floating.enabled = true;

        hookScript.DetachHook(this);
        gameObject.layer = LayerMask.NameToLayer("Crate");

        if (useTimer == true) UpdateMethod += Timer;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;


        fishermenInLegs = new System.Collections.Generic.List<FisherMan>();        

    }
    public void Chain(Rigidbody2D rb) {
        GrabHook(rb);
    }
}
public interface IChainable {
    void Chain(Rigidbody2D rb);
}
