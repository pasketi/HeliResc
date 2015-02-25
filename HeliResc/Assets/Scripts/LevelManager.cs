using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

	private int savedCrates, cargoCrates, crateAmount;
	private bool allSaved, reset = false;
	public float waterLevel = 0, uiLiftPowerWidth = 0.1f, uiLiftPowerDeadZone = 0.05f, resetCountdown = 3f;
	public Text cargoText, savedText;


	// Use this for initialization
	void Start () {
		crateAmount = countCrates ();
		savedText.text = savedCrates + "/" + crateAmount;
	}
	
	// Update is called once per frame
	void Update () {
		if (allSaved) {
			Debug.Log("Victory!");
			Reset();
		}
		if (reset) {
			resetCountdown -= Time.deltaTime;
			if (GameObject.Find("Copter") != null) GameObject.Find("Copter").GetComponent<CopterManagerTouch>().isKill = true;
			if (resetCountdown <= 0f) Application.LoadLevel(Application.loadedLevelName);
		}
	}

	public void Reset() {
		reset = true;
	}

	public void saveCrate(float crateMass) {
		savedCrates++;
		if (savedCrates >= crateAmount) {
			allSaved = true;
		}
		savedText.text = savedCrates + "/" + crateAmount;
		GameObject.Find("Copter").GetComponent<CopterManagerTouch>().dropOneCrate(crateMass);
	}

	private int countCrates (){
		var crates = GameObject.FindGameObjectsWithTag ("SaveableObject");
		/*int count = 0;
		foreach (var crate in crates) {
			count++;
		}*/
		return crates.Length;
	}

	public void cargoHookedCrates(GameObject hook){
		foreach (Transform child in hook.transform) {
			cargoCrates++;
			Destroy(child.gameObject);
		}
		cargoText.text = getCargoCrates().ToString();
	}

	public int getCargoCrates() {
		return cargoCrates;
	}

	public void emptyCargo() {
		savedCrates += cargoCrates;
		cargoCrates = 0;
		if (savedCrates >= crateAmount) {
			allSaved = true;
		}
		savedText.text = savedCrates + "/" + crateAmount;
		cargoText.text = "0";
		GameObject.Find("Copter").GetComponent<CopterManagerTouch>().dropAllCrates();
	}

	public float getWaterLevel(){
		return waterLevel;
	}

	public void setWaterLevel(float newWaterLevel) {
		waterLevel = newWaterLevel;
	}
}
