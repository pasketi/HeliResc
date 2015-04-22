using UnityEngine;
using System.Collections;

public class LandingPadManager : MonoBehaviour {

	public delegate void LandingEvent();
	public event LandingEvent enterPlatform;
	public event LandingEvent exitPlatform;

	private bool repair;
	private bool refill;

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
			if (enterPlatform != null) enterPlatform();
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
			if(repair) {
				if (other.GetComponent<CopterManagerTouch>().getHealth() < other.GetComponent<CopterManagerTouch>().getMaxHealth()) {
					other.GetComponent<CopterManagerTouch>().changeHealth((float)other.GetComponent<CopterManagerTouch>().getHealSpeed()*Time.deltaTime);
				}
			}
			if(refill) {
				if (other.GetComponent<CopterManagerTouch>().getFuel() < other.GetComponent<CopterManagerTouch>().getMaxFuel()) {
					other.GetComponent<CopterManagerTouch>().changeFuel(other.GetComponent<CopterManagerTouch>().getReFuelSpeed()*Time.deltaTime);
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.transform.tag == "Copter") {
			if (exitPlatform != null) exitPlatform();
		}
	}

	public void StartRepair() {
		repair = true;
	}
	
	public void StartRefill() {
		refill = true;
	}
}
