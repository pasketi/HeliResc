using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

	private int savedCrates = 0, crateAmount, stars = 0;
	private GameManager gameManager;
	private GameObject copter;

	private int reward = 1, actionsPerLevel = 0;
	private float graceTime = 10f;
	public int levelCoinRewardPerStar = 200, neededCrates = 0;
	public GameObject pauseScreen, HUD, copterSpawnPoint, kamikazePelican;
	public GameObject[] copters;
	private bool win = false, lose = false, splash = false, gamePaused = false, takenDamage = false, once = false, releaseThePelican = false;
	public float waterLevel = 0f, uiLiftPowerWidth = 0.1f, uiLiftPowerDeadZone = 0.05f, resetCountdown = 3f, crateSize, mapBoundsLeft = -50f, mapBoundsRight = 50f;
	public int cargoSize = 2, cargoCrates = 0, levelAction = 0, maxActionsPerLevel = 0;

	// Use this for initialization
	void Start () {
		if (GameObject.Find("GameManager") != null){
			Debug.Log("GameManager Found");
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			gameManager.load();
			//copters = new GameObject[gameManager.getCopterAmount()];
		}
		crateAmount = countCrates();
		//crateSize = getCrateScale();
		actionsPerLevel = maxActionsPerLevel;
		if (pauseScreen == null) pauseScreen = GameObject.Find("PauseScreen");
		if (HUD == null) HUD = GameObject.Find("HUD");

		//copter instantiate
		if (copterSpawnPoint == null) copterSpawnPoint = GameObject.Find ("CopterSpawn");
		if (gameManager != null) copter = Instantiate (copters[gameManager.getCurrentCopter()], copterSpawnPoint.transform.position, Quaternion.identity) as GameObject;
		else copter = Instantiate (copters[0], copterSpawnPoint.transform.position, Quaternion.identity) as GameObject;
		copter.name = "Copter";

		resetCountdown = 3f;
		pauseScreen.SetActive(false);
		HUD.SetActive(true);

		// We are SO sorry
		//Application.targetFrameRate = 30;
	}
	
	// Update is called once per frame
	void Update () {
		if (savedCrates >= crateAmount || Input.GetKeyDown(KeyCode.J)) {
			win = true;
		}

		if (win) {
			if (!once) {
				Debug.Log("Victory!");
				stars += 1;
				if (!isDamageTaken()) stars += 1;
				if (savedCrates == crateAmount) stars += 1;
				if (stars < 3) gameManager.sendLevelEndInfo(stars, levelCoinRewardPerStar * stars);
				else if (stars >= 3) gameManager.sendLevelEndInfo(stars, (levelCoinRewardPerStar * stars) + (levelCoinRewardPerStar / 2));
				once = true;
                winLevel();
			}
			resetCountdown -= Time.deltaTime;
			if (resetCountdown <= 0f) Application.LoadLevel ("MainMenu");
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

		if (GameObject.Find("Copter") != null && !releaseThePelican && (GameObject.Find("Copter").transform.position.x <= mapBoundsLeft || GameObject.Find("Copter").transform.position.x >= mapBoundsRight)){
			releaseThePelican = true;
			if (GameObject.Find("Copter").transform.position.x <= mapBoundsLeft) Instantiate (kamikazePelican, new Vector3(mapBoundsLeft - 25f, Random.value * 15f, 0f), Quaternion.identity);
			else if (GameObject.Find("Copter").transform.position.x >= mapBoundsRight) Instantiate (kamikazePelican, new Vector3(mapBoundsRight + 25f, Random.value * 15f, 0f), Quaternion.identity);
		}

		if (GameObject.Find("Copter") != null && !releaseThePelican && GameObject.Find("Copter").GetComponent<CopterManagerTouch>().getFuel () == 0f) {
			if (graceTime > 0f) graceTime -= Time.deltaTime;
			else {
				releaseThePelican = true;
				int dir = Random.Range(0,2);
				if (dir == 0) dir--;
				else dir = 1;
				Instantiate (kamikazePelican, new Vector3(GameObject.Find("Copter").transform.position.x + ((float) dir * 15f), Random.value * 15f, 0f), Quaternion.identity);
			}
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

	public bool isDamageTaken () {
		return takenDamage;
	}
	public void damageTaken () {
		takenDamage = true;
	}

	public bool getPaused () {
		return gamePaused;
	}

    private void winLevel() {
        LevelEndInfo end = new LevelEndInfo(true);
        end.itemsSaved = getSavedCrates();
        end.Reward = reward;

        MissionObjectives mo = GameObject.Find("Objectives").GetComponent<MissionObjectives>();
        if (mo == null) Debug.Log("Objectives not found");
        end.star1 = mo.LevelObjective1();
        end.star2 = mo.LevelObjective2();
        end.star3 = mo.LevelObjective3();

        gameManager.loadMainMenu(true, end, 1);
    }

	public void backToMainMenu () {
		Time.timeScale = 1f;
		gameManager.loadMainMenu (false, null);
	}

	public void Reset() {
		Application.LoadLevel(Application.loadedLevelName);
	}

	private int countCrates (){
		var crates = GameObject.FindGameObjectsWithTag ("SaveableObject");
		var actionableObjects = GameObject.FindGameObjectsWithTag ("ActionableObject");
		return crates.Length + actionableObjects.Length;
	}

	private float getCrateScale() {
		GameObject crates = GameObject.FindGameObjectWithTag ("Crate");
		return crates.transform.localScale.x;
	}

	public int getCrateAmount () {
		return crateAmount;
	}

	public void saveCrates (int amount) {
        gameManager.wallet.AddMoney(amount * reward);
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

	public int getActionsLeft() {
		return actionsPerLevel;
	}

	public void useAction() {
		actionsPerLevel--;
	}
}
