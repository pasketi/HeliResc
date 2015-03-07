using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

	private int savedCrates = 0, crateAmount;
	public GameObject pauseScreen, HUD;
	private bool win = false, lose = false, splash = false, gamePaused = false;
	public float waterLevel = 0f, uiLiftPowerWidth = 0.1f, uiLiftPowerDeadZone = 0.05f, resetCountdown = 3f, crateSize;
	public int cargoSize = 2, cargoCrates = 0;

	// Use this for initialization
	void Start () {
		crateAmount = countCrates();
		crateSize = getCrateScale();
		if (pauseScreen == null) pauseScreen = GameObject.Find("PauseScreen");
		if (HUD == null) HUD = GameObject.Find("HUD");
		pauseScreen.SetActive(false);
		HUD.SetActive(true);

		// We are SO sorry
		//Application.targetFrameRate = 30;
	}
	
	// Update is called once per frame
	void Update () {
		if (savedCrates >= crateAmount) {
			win = true;
		}

		if (win) {
			Debug.Log("Victory!");
			Reset();
		}
		if (lose) {
			resetCountdown -= Time.deltaTime;
			if (GameObject.Find("Copter") != null) GameObject.Find("Copter").GetComponent<CopterManagerTouch>().isKill = true;
			if (resetCountdown <= 0f) Reset ();
		} else if (splash) {
			resetCountdown -= Time.deltaTime;
			if (GameObject.Find("Copter") != null) GameObject.Find("Copter").GetComponent<CopterManagerTouch>().isSplash = true;
			if (resetCountdown <= 0f) Reset ();
		}
	}

	public void levelFailed (int type) {
		if (type == 1)
			lose = true;
		else if (type == 2)
			splash = true;
	}

	public void levelPassed () {
		win = true;
	}

	public void pause () {
		if (!gamePaused) {
			gamePaused = true;

			HUD.SetActive(false);
			pauseScreen.SetActive(true);

			Time.timeScale = 0f;
		} else {
			gamePaused = false;

			pauseScreen.SetActive(false);
			HUD.SetActive(true);

			Time.timeScale = 1f;
		}
	}

	public bool getPaused () {
		return gamePaused;
	}

	public void Reset() {
		Application.LoadLevel(Application.loadedLevelName);
	}

	private int countCrates (){
		var crates = GameObject.FindGameObjectsWithTag ("SaveableObject");
		return crates.Length;
	}

	private float getCrateScale() {
		GameObject crates = GameObject.FindGameObjectWithTag ("Crate");
		return crates.transform.localScale.x;
	}

	public int getCrateAmount () {
		return crateAmount;
	}

	public void saveCrates (int amount) {
		savedCrates += amount;
	}

	public int getSavedCrates () {
		return savedCrates;
	}

	public float getWaterLevel(){
		return waterLevel;
	}

	public void setWaterLevel(float newWaterLevel) {
		waterLevel = newWaterLevel;
	}

	public void setCargoCrates(int amount) {
		cargoCrates = amount;
	}
}
