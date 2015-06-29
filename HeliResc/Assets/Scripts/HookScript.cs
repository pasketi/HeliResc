using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HookScript : MonoBehaviour {

	private LineRenderer line;                  //The rope to draw
	private Transform anchor;                   //The anchor point
    private Transform _t;                       //Reference to own transform
    private Rigidbody2D rb;                     //Reference to own rigidbody
    private List<SaveableObject> hookedItems;   //Items that are hanging in the hook

    //Getter for the list
    public List<SaveableObject> HookedItems { get { return hookedItems; } }

    //Start listening to events
    void OnEnable() {
        EventManager.StartListening("CopterExplode", DisableLine);
        EventManager.StartListening("EnterPlatform", DisableLine);
    }
    void OnDisable() {
        EventManager.StopListening("CopterExplode", DisableLine);
        EventManager.StopListening("EnterPlatform", DisableLine);
    }

	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
		line.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;    //Set the line to be drawn under hook
		line.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
        rb = GetComponent<Rigidbody2D>();
        anchor = GameObject.FindGameObjectWithTag("CopterHookLineAnchor").transform;
        _t = transform;

        hookedItems = new List<SaveableObject>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateLine();	
        UpdateRotation();                
	}

    private void UpdateLine() {
        line.SetPosition(0, _t.position);
        line.SetPosition(1, anchor.position);
    }
    private void UpdateRotation() {
        float angle = Vector3.Angle(Vector3.up, (anchor.position - _t.position));

        if (_t.position.x >= anchor.position.x)
            _t.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        else
            _t.rotation = Quaternion.Euler(new Vector3(0, 0, 360 - angle));
    }
    public void GrabHook(SaveableObject obj) {
        if(hookedItems.Contains(obj) == false)
            hookedItems.Add(obj);
    }
    public void DetachHook(SaveableObject obj) {
        if(hookedItems.Contains(obj) == true)
            hookedItems.Remove(obj);
    }
    private void DisableLine() {
        line.enabled = false;
    }
    private void EnableLine() {
        line.enabled = true;
    }
}
