using UnityEngine;
using System.Collections;

public class HookLineManager : MonoBehaviour {

	private LineRenderer line;
	private GameObject anchor;

	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
		line.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
		line.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;

        anchor = GameObject.FindGameObjectWithTag("CopterHookLineAnchor");
	}
	
	// Update is called once per frame
	void Update () {		

		line.SetPosition (0, gameObject.transform.position);
		line.SetPosition (1, anchor.transform.position);

        //if (GameObject.Find ("Copter") != null && GameObject.Find ("Copter").GetComponent<CopterManagerTouch> ().isHookDead) {
        //    Destroy (line);
        //    Destroy (this);
        //}
	}
}
