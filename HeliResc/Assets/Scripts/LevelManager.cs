using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

	private int savedCrates = 0, crateAmount, stars = 0;
	private GameManager gameManager;
	private GameObject copter;

    private MissionObjectives objectives;

    private Copter copterScript;

	private int reward = 1, actionsPerLevel = 0;
	private float graceTime = 10f;
	public int levelCoinRewardPerStar = 200, neededCrates = 0;
	public GameObject pauseScreen, HUD, copterSpawnPoint, kamikazePelican;
	public GameObject levelSplash;
	private bool win = false, lose = false, splash = false, gamePaused = false, takenDamage = false, once = false, releaseThePelican = false;
	public float waterLevel = 0f, uiLiftPowerWidth = 0.1f, uiLiftPowerDeadZone = 0.05f, resetCountdown = 3f, crateSize, mapBoundsLeft = -50f, mapBoundsRight = 50f;
	public int cargoSize = 2, cargoCrates = 0, levelAction = 0, maxActionsPerLevel = 0;

    void OnEnable() {
        EventManager.StartListening("CopterExplode", CopterExploded);
        EventManager.StartListening("CopterSplash", CopterSplashed);
        EventManager.StartListening(SaveStrings.escape, backButton);
    }
    void OnDisable() {
        EventManager.StopListening("CopterExplode", CopterExploded);
        EventManager.StopListening("CopterSplash", CopterSplashed);
        EventManager.StopListening(SaveStrings.escape, backButton);
    }

	// Use this for initialization
	void Awake () {
		if (GameObject.Find("GameManager") != null){

			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			gameManager.load();
			//copters = new GameObject[gameManager.getCopterAmount()];
		}
		crateAmount = countCrates();
		//crateSize = getCrateScale();
		actionsPerLevel = maxActionsPerLevel;
		if (pauseScreen == null) pauseScreen = GameObject.Find("PauseScreen");
		if (HUD == null) HUD = GameObject.Find("HUD");

        objectives = GameObject.Find("Objectives").GetComponent<MissionObjectives>();
        objectives.OnGameOver += levelFinished;

        //copter instantiate
        if (copterSpawnPoint == null) copterSpawnPoint = GameObject.Find ("CopterSpawn");
		if (gameManager != null) copter = Instantiate (gameManager.CurrentCopter, copterSpawnPoint.transform.position, Quaternion.identity) as GameObject;
		//else copter = Instantiate (copters[0], copterSpawnPoint.transform.position, Quaternion.identity) as GameObject;
		copter.name = "Copter";
        copterScript = copter.GetComponent<Copter>();
        cargoSize = copterScript.cargo.maxCapacity;
        

		resetCountdown = 3f;
		//pauseScreen.SetActive(false);
		//HUD.SetActive(true);



		// We are SO sorry
		//Application.targetFrameRate = 30;
	}
	
	// Update is called once per frame
	void Update () {		
		if (win) {
			if (!once) {

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
        //if (lose) {
        //    resetCountdown -= Time.deltaTime;
        //    //if (GameObject.Find("Copter") != null) GameObject.Find("Copter").GetComponent<CopterManagerTouch>().isKill = true;
        //    if (resetCountdown <= 0f) loseLevel (EndReason.explode);
        //} else if (splash) {
        //    resetCountdown -= Time.deltaTime;
        //    //if (GameObject.Find("Copter") != null) GameObject.Find("Copter").GetComponent<CopterManagerTouch>().isSplash = true;
        //    if (resetCountdown <= 0f) loseLevel(EndReason.drowned);
        //}

		if (GameObject.Find("Copter") != null && !releaseThePelican && (GameObject.Find("Copter").transform.position.x <= mapBoundsLeft || GameObject.Find("Copter").transform.position.x >= mapBoundsRight)){
			releaseThePelican = true;
			if (GameObject.Find("Copter").transform.position.x <= mapBoundsLeft) Instantiate (kamikazePelican, new Vector3(mapBoundsLeft - 25f, Random.value * 15f, 0f), Quaternion.identity);
			else if (GameObject.Find("Copter").transform.position.x >= mapBoundsRight) Instantiate (kamikazePelican, new Vector3(mapBoundsRight + 25f, Random.value * 15f, 0f), Quaternion.identity);
		}

		if (GameObject.Find("Copter") != null && !releaseThePelican && GameObject.Find("Copter").GetComponent<Copter>().fuelTank.CurrentFuel <= 0f) {
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

    private void CopterExploded() { 
        lose = true;
        Invoke("loseLevel", resetCountdown);
    }
    private void CopterSplashed() { 
        splash = true;
        Invoke("loseLevel", resetCountdown);
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

	public bool allCratesCollected() {
		return (savedCrates >= crateAmount);
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
    public void backButton() {
        if (gamePaused == true) {
            Application.LoadLevel("LevelMap");
        } else {
            pause();
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

    private void levelFinished() {
        winLevel();
    }

    public void pressFinishButton() {
        winLevel();
    }

    private void winLevel() {

        int condition = objectives.AllObjectiveCompleted() ? EndReason.winner : EndReason.passed;

        LevelEndInfo end = new LevelEndInfo(true, condition);
        end.itemsSaved = getSavedCrates();
        end.Reward = reward;
                
        if (objectives == null) Debug.Log("Objectives not found");

        end.star1 = objectives.LevelObjective1();
        end.star2 = objectives.LevelObjective2();
        end.star3 = objectives.LevelObjective3();

        gameManager.loadMainMenu(true, end, 2);
    }

    private void loseLevel() {
        int loseCondition = 0;
        if(lose == true) loseCondition = EndReason.explode;
        else if(splash == true) loseCondition = EndReason.drowned;


        LevelEndInfo end = new LevelEndInfo(false, loseCondition);
        gameManager.loadMainMenu(true, end, 2);
    }

	public void backToMainMenu () {
		Time.timeScale = 1f;
		gameManager.loadMainMenu (false, null);
	}

	public void Reset() {
		Application.LoadLevel(Application.loadedLevelName);
	}

	private int countCrates (){
		//var crates = GameObject.FindGameObjectsWithTag ("SaveableObject");
		//var actionableObjects = GameObject.FindGameObjectsWithTag ("ActionableObject");

        var crates = GameObject.FindObjectsOfType<SaveableObject>();
        var actionableObjects = GameObject.FindObjectsOfType<ActionableObject>();

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

    //public void setWaterLevel(float newWaterLevel) {
    //    waterLevel = newWaterLevel;
    //}

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
