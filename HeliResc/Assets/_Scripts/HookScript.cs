using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HookScript : MonoBehaviour {

	private LineRenderer line;                  //The rope to draw
	private Transform anchor;                   //The anchor point
    private Transform _t;                       //Reference to own transform
    private Rigidbody2D rb;                     //Reference to own rigidbody
    private List<HookableObject> hookedItems;   //Items that are hanging in the hook


    //Getter for the list
    public List<HookableObject> HookedItems { get { return hookedItems; } }
    public float HookMass { 
        set {
            if (rb == null)
                rb = GetComponent<Rigidbody2D>();
            rb.mass = value; 
        } 
        get { 
            return rb.mass; 
        }
    }

    //Start listening to events
    protected virtual void OnEnable() {
        EventManager.StartListening(SaveStrings.eCopterExplode, DisableLine);
		EventManager.StartListening (SaveStrings.eHookDied, HookDied);
        //EventManager.StartListening("EnterPlatform", DisableLine);
    }
    protected virtual void OnDisable() {
        EventManager.StopListening(SaveStrings.eCopterExplode, DisableLine);
		EventManager.StopListening (SaveStrings.eHookDied, HookDied);
		//EventManager.StopListening("EnterPlatform", DisableLine);
    }

	// Use this for initialization
	protected virtual void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
		line.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;    //Set the line to be drawn under hook
		line.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
        rb = GetComponent<Rigidbody2D>();
        anchor = GameObject.Find("Copter").transform.Find("Anchor");
        _t = transform;        

        ResetHook(new List<HookableObject>());
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        UpdateLine();	
        UpdateRotation();                
	}

    protected virtual void UpdateLine()
    {
        line.SetPosition(0, _t.position);
        line.SetPosition(1, anchor.position);
    }
    protected virtual void UpdateRotation()
    {
        float angle = Vector3.Angle(Vector3.up, (anchor.position - _t.position));

        if (_t.position.x >= anchor.position.x)
            _t.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        else
            _t.rotation = Quaternion.Euler(new Vector3(0, 0, 360 - angle));
    }
    public void ResetHook(List<HookableObject> newList) {
        if (newList.Count > 0)
        {
            if (newList[0] is IChainable)
            {
                Rigidbody2D r = rb;
                foreach (HookableObject so in newList)
                {
                    IChainable chain = so as IChainable;
                    chain.Chain(r);
                    foreach (Transform t in so.transform)
                    {
                        if (t.tag.Equals("Hook"))
                            r = t.GetComponent<Rigidbody2D>();
                    }
                }
            }
        }
        hookedItems = newList;
    }
            
	protected virtual void HookDied() {
		int count = hookedItems.Count;
		for(int i = 0; i < count; i++) {
			hookedItems[0].DetachHook();
		}
	}

    public virtual void GrabHook(HookableObject obj) {
        if (hookedItems.Contains(obj) == false) {
            HookMass += obj.mass;
            hookedItems.Add(obj);
        }
    }
    public virtual void DetachHook(HookableObject obj) {
        if (hookedItems.Contains(obj) == true) {
            HookMass -= obj.mass;
            hookedItems.Remove(obj);
        }
    }
    public virtual void DisableLine() {
        line.enabled = false;
    }
    public virtual void EnableLine() {
        line.enabled = true;
    }
}
