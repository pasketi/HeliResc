using UnityEngine;
using System.Collections;

public class LandingPadManager : MonoBehaviour {

	//private BoxCollider2D trigger;
	private CargoManager cargo;

	// Use this for initialization
	void Start () {
		cargo = GameObject.Find ("Copter").GetComponent<CargoManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.transform.tag == "Crate") {
			cargo.saveCrate(other.GetComponentInChildren<CrateManager>().crateMass);
			Destroy(other.gameObject);
		}
		if (other.gameObject.transform.tag == "Copter" && cargo.getCargoCrates() > 0) {
			cargo.emptyCargo();
			other.GetComponent<CopterManagerTouch>().resetPower();
		}
		if (other.gameObject.transform.tag == "Copter" && other.GetComponent<CopterManagerTouch>().isHookDead == true) {
			other.GetComponent<CopterManagerTouch>().isHookDead = false;
		}
	}
}
