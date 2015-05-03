using UnityEngine;
using System.Collections;

public class CargoManager : MonoBehaviour {

	private LevelManager manager;
	private CopterManagerTouch copterScript;
	private Transform cargo;
	private int cargoCrates = 0;
	private float copterMass, cargoMass = 0f, hookMass = 0f;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find("LevelManagerO").GetComponent<LevelManager>();
		copterScript = GameObject.Find ("Copter").GetComponent<CopterManagerTouch> ();
		cargo = gameObject.transform.FindChild("Cargo");
		copterMass = GetComponent<Rigidbody2D>().mass;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<Rigidbody2D>().mass = copterMass + cargoMass + hookMass;
		copterScript.testHookMass(hookMass);
	}

	public void changeCargoMass (float crateMass) {
		cargoMass += crateMass;
	}

	public void changeHookMass (float crateMass) {
		hookMass += crateMass;
	}

	public void dropAllCrates () {
		cargoMass = 0f;
		hookMass = 0f;
		foreach(Transform crate in cargo) {
			Destroy(crate.gameObject);
		}
	}

	public void cargoHookedCrates(GameObject hook){
		foreach (Transform child in hook.transform) {

			if (child.FindChild ("LegHook") != null && child.FindChild("LegHook").childCount != 0)
				cargoHookedCrates (child.FindChild("LegHook").gameObject);
			if (manager.cargoSize > cargoCrates){
				if (child.tag == "Crate"){
					changeCargoMass(child.GetComponentInChildren<CrateManager>().crateMass);
					changeHookMass(-child.GetComponentInChildren<CrateManager>().crateMass);
					cargoCrates += 1;
					child.GetComponentInChildren<CrateManager>().inCargo = true;
					child.parent = cargo;
					child.GetComponent<Collider2D>().enabled = false;
					child.GetComponent<Renderer>().enabled = false;
					child.GetComponent<Rigidbody2D>().isKinematic = true;
					child.transform.localPosition = Vector3.zero;
					manager.setCargoCrates(cargoCrates);
				}
			} else {/*Fix the weird physics here*/}
		}
	}

	public void saveHookedCrate(float crateMass) {
		manager.saveCrates(1);
		changeHookMass(-crateMass);
	}
	
	public int getCargoCrates() {
		return cargoCrates;
	}
	
	public void emptyCargo() {
		manager.saveCrates(cargoCrates);
		cargoCrates = 0;
		manager.setCargoCrates(0);
		dropAllCrates();
	}

	public void transferAllCargo(Transform destination) {
		foreach (Transform cargoObject in cargo) {
			if (cargoObject.tag == "Crate") {
				cargoObject.parent = destination;
			}
		}
	}
}
