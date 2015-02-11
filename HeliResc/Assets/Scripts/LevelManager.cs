using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	private int savedCrates, cargoCrates, crateAmount;
	private bool allSaved;
	public float waterLevel = 0;
	
	// Use this for initialization
	void Start () {
		crateAmount = countCrates ();
	}
	
	// Update is called once per frame
	void Update () {
		if (allSaved) {
			Debug.Log("Victory!");
			Reset();
		}
	}

	public void Reset() {
		Application.LoadLevel(Application.loadedLevelName);
	}

	public void saveCrate() {
		savedCrates++;
		if (savedCrates >= crateAmount) {
			allSaved = true;
		}
		Debug.Log ("Crate Saved! Current score: " + savedCrates + "/" + crateAmount);
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
		}
	}

	public int getCargoCrates() {
		return cargoCrates;
	}

	public void emptyCargo() {
		savedCrates += cargoCrates;
		cargoCrates = 0;
		Debug.Log ("Cargo Emptied! Current score: " + savedCrates + "/" + crateAmount);
		if (savedCrates >= crateAmount) {
			allSaved = true;
		}
	}

	public float getWaterLevel(){
		return waterLevel;
	}

	public void setWaterLevel(float newWaterLevel) {
		waterLevel = newWaterLevel;
	}
}
