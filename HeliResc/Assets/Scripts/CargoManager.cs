using UnityEngine;
using System.Collections;

public class CargoManager : MonoBehaviour {

	private LevelManager manager;
	private Transform cargo;
	private int cargoCrates = 0;
	private float copterMass, cargoMass = 0f;

	// Use this for initialization
	void Start () {
		manager = (LevelManager) GameObject.Find("LevelManagerO").GetComponent(typeof(LevelManager));
		cargo = gameObject.transform.FindChild("Cargo");
		copterMass = rigidbody2D.mass;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.rigidbody2D.mass = copterMass + cargoMass;
	}

	public void changeCargoMass (float crateMass) {
		cargoMass += crateMass;
	}

	public void dropAllCrates () {
		gameObject.rigidbody2D.mass = copterMass;
		cargoMass = 0f;
		foreach(Transform crate in cargo) {
			Destroy(crate.gameObject);
		}
	}

	public void cargoHookedCrates(GameObject hook){
		foreach (Transform child in hook.transform) {
			if (manager.cargoSize > cargoCrates){
				if (child.tag == "Crate"){
					cargoCrates += 1;
					child.parent = cargo;
					child.collider2D.enabled = false;
					child.renderer.enabled = false;
					manager.setCargoCrates(cargoCrates);
				}
			}
		}
	}

	public void saveCrate(float crateMass) {
		manager.saveCrates(1);
		changeCargoMass(-crateMass);
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
