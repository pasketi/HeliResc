using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

	private int savedCrates = 0, crateAmount, stars = 0;
	private GameManager gameManager;
	private GameObject copter;

	public GameState gameState;								//Keeps track of the state of the game. Is the level in pregame, running or postgame
	public float pregameTimer;								//How long should the pregame last
	public float LevelTimer { get { return levelTimer; } }	//Getter for the level timer
	private float levelTimer;								//The time when gamestate is changed to running

    private MissionObjectives objectives;

    private Copter copterScript;

	private int reward = 1, actionsPerLevel = 0;
	private float graceTime = 5f, maxGraceTime = 5f;
	public int levelCoinRewardPerStar = 200, neededCrates = 0;
	public GameObject pauseScreen, HUD, copterSpawnPoint, kamikazePelican;
	public SoundObject endCheer;
	private bool cheerOnce = false;
	public GameObject levelSplash;
	private bool win = false, lose = false, splash = false, gamePaused = false, takenDamage = false, once = false, releaseThePelican = false;
	private bool exploded;
	public float waterLevel = 0f, uiLiftPowerWidth = 0.1f, uiLiftPowerDeadZone = 0.05f, resetCountdown = 3f, crateSize, mapBoundsLeft = -50f, mapBoundsRight = 50f;
	public int cargoSize = 2, cargoCrates = 0, levelAction = 0, maxActionsPerLevel = 0;
	private GameObject resetButton;

    void OnEnable() {
        EventManager.StartListening(SaveStrings.eCopterExplode, CopterExploded);
        EventManager.StartListening(SaveStrings.eCopterSplash, CopterSplashed);
        EventManager.StartListening(SaveStrings.eEscape, backButton);
    }
    void OnDisable() {
        EventManager.StopListening(SaveStrings.eCopterExplode, CopterExploded);
        EventManager.StopListening(SaveStrings.eCopterSplash, CopterSplashed);
        EventManager.StopListening(SaveStrings.eEscape, backButton);
    }

	// Use this for initialization
	void Awake () {
		gameState = GameState.PreGame;

		if (GameObject.Find("GameManager") != null){

			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			gameManager.load();
			//copters = new GameObject[gameManager.getCopterAmount()];
		}
		crateAmount = countCrates();
		actionsPerLevel = maxActionsPerLevel;
		if (pauseScreen == null) pauseScreen = GameObject.Find("PauseScreen");
		if (HUD == null) HUD = GameObject.Find("HUD");
		if (resetButton == null) {
			resetButton = HUD.transform.FindChild("ResetButton").gameObject;
			resetButton.SetActive(false);
		}

        objectives = GameObject.FindObjectOfType<MissionObjectives>();
		        
		//copter instantiate
        if (copterSpawnPoint == null) copterSpawnPoint = GameObject.Find ("CopterSpawn");
		if (gameManager != null) copter = Instantiate (gameManager.CurrentCopter, copterSpawnPoint.transform.position, Quaternion.identity) as GameObject;

		copter.name = "Copter";
		copterScript = copter.GetComponent<Copter>();
        cargoSize = copterScript.cargo.maxCapacity;
        

		resetCountdown = 3f;
		if (GameObject.FindObjectOfType<TutorialScript> () == null) {
			StartCoroutine (PreGame ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (gameState.Equals (GameState.Running)) {
			levelTimer += Time.deltaTime;
		}
        //if (win) {
        //    if (!once) {

        //        stars += 1;
        //        if (!isDamageTaken()) stars += 1;
        //        if (savedCrates == crateAmount) stars += 1;
        //        once = true;
        //        winLevel();
        //    }
        //    resetCountdown -= Time.deltaTime;
        //    if (resetCountdown <= 0f) Application.LoadLevel ("MainMenu");
        //}
        //if (lose) {
        //    resetCountdown -= Time.deltaTime;
        //    //if (copter != null) copter.GetComponent<CopterManagerTouch>().isKill = true;
        //    if (resetCountdown <= 0f) loseLevel (EndReason.explode);
        //} else if (splash) {
        //    resetCountdown -= Time.deltaTime;
        //    //if (copter != null) copter.GetComponent<CopterManagerTouch>().isSplash = true;
        //    if (resetCountdown <= 0f) loseLevel(EndReason.drowned);
        //}

		if (copter != null && !releaseThePelican && (copter.transform.position.x <= mapBoundsLeft || copter.transform.position.x >= mapBoundsRight)){
			releaseThePelican = true;
			if (copter.transform.position.x <= mapBoundsLeft) Instantiate (kamikazePelican, new Vector3(mapBoundsLeft - 25f, Random.value * 15f, 0f), Quaternion.identity);
			else if (copter.transform.position.x >= mapBoundsRight) Instantiate (kamikazePelican, new Vector3(mapBoundsRight + 25f, Random.value * 15f, 0f), Quaternion.identity);
		}

		if (copter != null && !releaseThePelican && copterScript.fuelTank.CurrentFuel <= 0f) {
			if (graceTime > 0f) {
				if (Mathf.Abs(copter.GetComponent<Rigidbody2D>().velocity.magnitude) <= 1f) graceTime -= Time.deltaTime;
			}
			else if (graceTime <= 0f) {
				releaseThePelican = true;
				int dir = Random.Range(0,2);
				if (dir == 0) dir--;
				else dir = 1;
				Instantiate (kamikazePelican, new Vector3(copter.transform.position.x + ((float) dir * 15f), Random.value * 15f, 0f), Quaternion.identity);
			}
		}
		else if (copterScript.fuelTank.currentFuel > 0f) {
			graceTime = maxGraceTime;
		}
	}

	private IEnumerator PreGame() {
		float deltaTime = Time.time;
		float timer = pregameTimer;
		while (timer > 0) {
			timer -= (Time.time - deltaTime);
			deltaTime = Time.time;
			yield return null;
		}
		StartGame ();
	}
	private IEnumerator PostGame(bool passed) {
        gameState = GameState.PostGame;
        int endReason = EndReason.lose;

        //Determine why the level ended        
        if (passed == true && objectives.AllObjectiveCompleted())
            endReason = EndReason.winner;
        else if (passed == true && objectives.AnyObjectiveCompleted())
            endReason = EndReason.passed;
        else if (exploded == true)
            endReason = EndReason.explode;
        else if (splash == true)
            endReason = EndReason.drowned;

        LevelEndInfo end = new LevelEndInfo(passed, endReason);

        end.level = LevelHandler.CurrentLevel;
        end.itemsSaved = getSavedCrates();
        end.Reward = reward;
        end.levelTime = levelTimer;
        if (passed == true) {
            LevelHandler.CompleteLevel(end.level);
            FireworksController fw = GameObject.FindObjectOfType<FireworksController>();
			if (!cheerOnce) {
				endCheer.PlaySound();
				cheerOnce = true;
			}
            if (fw != null) {
                //fw.transform.position = GameObject.FindObjectOfType<Copter>().transform.position + Vector3.up * 3;
                fw.Launch();
            }
        }
        
        RubyScript ruby = GameObject.FindObjectOfType<RubyScript>();

        if (ruby == null) Debug.LogError("Ruby not found");
        else { 
            end.rubyFound = ruby.found;
            end.sapphireFound = ruby.found && ruby.IsSapphire;
        }

        if (objectives == null) Debug.LogError("Objectives not found");
		else if (passed == true)
        {
            end.obj1Passed = objectives.LevelObjective1();
            end.obj2Passed = objectives.LevelObjective2();
            end.obj3Passed = objectives.LevelObjective3();
        }

		setResetButton(true);
        float timer = resetCountdown;
        float deltaTime = Time.time;
        while (timer > 0) {
            timer -= (Time.time - deltaTime);
            deltaTime = Time.time;
            yield return null;
        }

		if (SceneManager.GetActiveScene().name == "Tutorial00" && !passed) GameManager.LoadLevel("IntroScreen");
		else gameManager.loadMainMenu(true, end, 2);
	}

	public void StartGame() {
		gameState = GameState.Running;
	}    

    private void CopterCrashed() {
        if (gameState.Equals(GameState.Running)) {
            lose = true;
            StartCoroutine(PostGame(false));
        }
    }
    private void CopterExploded() {
        if (gameState.Equals(GameState.Running)) {
            lose = true;
            exploded = true;
            StartCoroutine(PostGame(false));
        }
    }
    private void CopterSplashed() {
        if (gameState.Equals(GameState.Running)) {
            lose = true;
            splash = true;
            StartCoroutine(PostGame(false));
        }
    }

    public void LoseLevel() {
        if (gameState.Equals(GameState.Running)) {
            lose = true;
            StartCoroutine(PostGame(false));
        }
    }

    public void winLevel()
    {
        StartCoroutine(PostGame(true));
    }

    private void loseLevel()
    {
        //int loseCondition = 0;
        //if (lose == true) loseCondition = EndReason.lose;
        //if (splash == true) loseCondition = EndReason.drowned;
        //if (exploded == true) loseCondition = EndReason.explode;


        //LevelEndInfo end = new LevelEndInfo(false, loseCondition);
        //end.level = LevelHandler.CurrentLevel;
        //end.passedLevel = false;

        //RubyScript ruby = GameObject.FindObjectOfType<RubyScript>();
        //end.rubyFound = ruby.found;

        //gameManager.loadMainMenu(true, end, 2);
    }	

	public bool allCratesCollected() {
		return (savedCrates >= crateAmount);
	}

	public void pause () {
		if (!gamePaused) {
			gamePaused = true;

			if(gameState.Equals(GameState.Running)) {
				gameState = GameState.Paused;
			}

			HUD.SetActive(false);
			pauseScreen.SetActive(true);

			Time.timeScale = 0f;
		} else {
			gamePaused = false;

			if(gameState.Equals(GameState.Paused)) {
				gameState = GameState.Running;
			}

			pauseScreen.SetActive(false);
			HUD.SetActive(true);

			Time.timeScale = 1f;
		}
	}
    public void backButton() {
        if (gamePaused == true) {
            GameManager.LoadLevel("LevelMap");
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

	public void backToMainMenu () {
		Time.timeScale = 1f;
		gameManager.loadMainMenu (false, null);
	}

	public void Reset() {
        GameManager.LoadLevel(Application.loadedLevelName);
	}

	private int countCrates (){
		//var crates = GameObject.FindGameObjectsWithTag ("SaveableObject");
		//var actionableObjects = GameObject.FindGameObjectsWithTag ("ActionableObject");

        var crates = GameObject.FindObjectsOfType<HookableObject>();
        var actionableObjects = GameObject.FindObjectsOfType<ActionableObject>();

        int crateAmount = 0;
        foreach (HookableObject ho in crates) {
            if (ho.saveable)
                crateAmount++;
        }

		return crateAmount + actionableObjects.Length;
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

	public void setResetButton(bool a) {
		resetButton.SetActive(a);
	}

	public bool getResetButton() {
		return resetButton.activeSelf;
	}
}

public enum GameState {
	Running,
	Paused,
	PreGame,
	PostGame
}