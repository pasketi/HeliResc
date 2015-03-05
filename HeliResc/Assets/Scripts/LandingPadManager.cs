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
			cargo.saveHookedCrate(other.GetComponentInChildren<CrateManager>().crateMass);
			Destroy(other.gameObject);
		}

		if (other.gameObject.transform.tag == "Copter") {
		    if (cargo.getCargoCrates() > 0) {
				cargo.emptyCargo();
				other.GetComponent<CopterManagerTouch>().resetPower();
			}

			if (other.GetComponent<CopterManagerTouch>().isHookDead == true) {
				other.GetComponent<CopterManagerTouch>().isHookDead = false;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.transform.tag == "Copter") {
			if (other.GetComponent<CopterManagerTouch>().getHealth() < other.GetComponent<CopterManagerTouch>().maxHealth) {
				other.GetComponent<CopterManagerTouch>().changeHealth((float)other.GetComponent<CopterManagerTouch>().healPerSecond*Time.deltaTime);
			}
			if (other.GetComponent<CopterManagerTouch>().getFuel() < other.GetComponent<CopterManagerTouch>().maxFuel) {
				other.GetComponent<CopterManagerTouch>().changeFuel(other.GetComponent<CopterManagerTouch>().reFuelPerSecond*Time.deltaTime);
			}
		}
	}
}
