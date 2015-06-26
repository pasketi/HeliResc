using UnityEngine;
using System.Collections;

public class HookLineManager : MonoBehaviour {

	private LineRenderer line;
	private Transform anchor;
    private Transform _t;
    private Rigidbody2D rb;

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
		line.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
		line.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
        rb = GetComponent<Rigidbody2D>();
        anchor = GameObject.FindGameObjectWithTag("CopterHookLineAnchor").transform;
        _t = transform;
	}
	
	// Update is called once per frame
	void Update () {		

		line.SetPosition (0, _t.position);
		line.SetPosition (1, anchor.position);        

        float angle = Vector3.Angle(Vector3.up, (anchor.position - _t.position));       
        
        if(_t.position.x >= anchor.position.x)
            _t.rotation = Quaternion.Euler(new Vector3(0,0, angle));
        else
            _t.rotation = Quaternion.Euler(new Vector3(0, 0, 360 - angle));
        
        //if (GameObject.Find ("Copter") != null && GameObject.Find ("Copter").GetComponent<CopterManagerTouch> ().isHookDead) {
        //    Destroy (line);
        //    Destroy (this);
        //}
	}

    private void DisableLine() {
        line.enabled = false;
    }
    private void EnableLine() {
        line.enabled = true;
    }
}
