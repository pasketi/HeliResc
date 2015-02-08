using UnityEngine;
using System.Collections;

public class LandingPadManager : MonoBehaviour {

	//private BoxCollider2D trigger;
	private LevelManager manager;

	// Use this for initialization
	void Start () {
		manager = (LevelManager) GameObject.Find("LevelManagerO").GetComponent(typeof(LevelManager));
		//trigger = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.transform.tag == "Crate") {
			Destroy(other.gameObject);
			manager.saveCrate();
		}
		if (other.gameObject.transform.tag == "Copter") {
			manager.emptyCargo();
		}
	}
}
