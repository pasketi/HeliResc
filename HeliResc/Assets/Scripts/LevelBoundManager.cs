using UnityEngine;
using System.Collections;

public class LevelBoundManager : MonoBehaviour {

	private LineRenderer line;
	private LevelManager manager;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find("LevelManagerO").GetComponent<LevelManager>();
		line = gameObject.GetComponent<LineRenderer> ();
		line.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
		line.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
		if (gameObject.name == "Left") {
			line.SetPosition (0, new Vector3 (manager.mapBoundsLeft + 10f, -5f));
			line.SetPosition (1, new Vector3 (manager.mapBoundsLeft + 10f, 200f));
		} else {
			line.SetPosition (0, new Vector3 (manager.mapBoundsRight - 10f, -5f));
			line.SetPosition (1, new Vector3 (manager.mapBoundsRight - 10f, 200f));
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
