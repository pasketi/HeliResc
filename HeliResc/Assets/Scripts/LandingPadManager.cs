using UnityEngine;
using System.Collections;

public class LandingPadManager : MonoBehaviour {

	//private BoxCollider2D trigger;
	private LevelManager manager;

	// Use this for initialization
	void Start () {
		manager = (LevelManager) GameObject.Find("LevelManagerO").GetComponent(typeof(LevelManager));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.transform.tag == "Crate") {
			manager.saveCrate(other.GetComponentInChildren<CrateManager>().crateMass);
			Destroy(other.gameObject);
		}
		if (other.gameObject.transform.tag == "Copter" && manager.getCargoCrates() > 0) {
			manager.emptyCargo();
			other.GetComponent<CopterManagerTouch>().resetPower();
		}
		if (other.gameObject.transform.tag == "Copter" && other.GetComponent<CopterManagerTouch>().isHookDead == true) {
			other.GetComponent<CopterManagerTouch>().isHookDead = false;
		}
	}
}
