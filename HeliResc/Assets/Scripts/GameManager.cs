using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	private bool showLevelEnd = false;
	public bool ShowLevelEnd { get { return showLevelEnd; } }

    public Wallet wallet;
	public LevelEndInfo levelEnd;

	private int currentMenu = 0;
	public int CurrentMenu { get { return currentMenu; } }


	void Awake(){
		DontDestroyOnLoad(gameObject);
        wallet = SaveLoad.LoadWallet();
		Debug.Log ("GameManager awake");
	}

	private const int copterAmount = 2; //FOR EVERY NEW COPTER, ADD 1 HERE
    public int CopterAmount { get { return copterAmount; } }

	private string 	playerName = "Anonymous";
	private int		playerFirst = 0,
					playerStars = 0, 
					playerCoins = 0, 
					playerPlatform = 1,
					currentCopter = 0,
					lastStars = 0,
					lastCoins = 0;

	/*
	 * An array of Copter statistics (IN STRING!!!)
	 * First number goes as the CopterNumber (0 being DEFAULT)
	 * I couldn't make it visible to the inspector, so we'll have to work with this file
	 * Second number used as follows:
	 * 
	 * 0. CopterName
	 * 1. CopterCost
	 * 2. CopterPlatform
	 * 3. CopterUnlocked 		-> Save
	 * 4. CopterEngineLevel 	-> Save
	 * 5. CopterFuelTankLevel 	-> Save
	 * 6. CopterEngineDefaultPower
	 * 7. CopterEnginePowerMax
	 * 8. CopterFuelTankDefaultValue
	 * 9. CopterFuelTankMaxValue
	 * 10. CopterWeight
	 * 11. CargoSize
	 * 12. RopeLevel			-> Save
	 * 13. RopeDefaultValue
	 * 14. RopeMaxValue
	 * 15. CopterMaxHealth
	 * 16. TiltSpeed
	 * 17. MaxTilt
	 * 
	 */
	private string[,] copters = new string[copterAmount,18]{
		{
			"DefaultCopter", 	//NAME
			"0", 				//COST
			"1", 				//PLATFORM
			"1", 				//UNLOCKED (0/1)
			"1", 				//ENGINE LEVEL
			"1", 				//FUELTANK LEVEL
			"100", 				//ENGINE DEFAULT VALUE
			"200",				//ENGINE MAX VALUE
			"300", 				//FUELTANK DEFAULT VALUE
			"500",				//FUELTANK MAX VALUE
			"15",				//WEIGHT
			"2",				//CARGOSIZE
			"1",				//ROPE LEVEL
			"5",				//ROPE DEFAULT VALUE
			"15",				//ROPE MAX VALUE
			"100",				//MAXHEALTH / DURABILITY
			"100",				//MAX TILT SPEED
			"75"				//MAX TILT VALUE
		},{
			"WaterCopter",	 	//NAME
			"0", 				//COST
			"1", 				//PLATFORM
			"1", 				//UNLOCKED (0/1)
			"1", 				//ENGINE LEVEL
			"1", 				//FUELTANK LEVEL
			"100", 				//ENGINE DEFAULT VALUE
			"200",				//ENGINE MAX VALUE
			"300", 				//FUELTANK DEFAULT VALUE
			"500",				//FUELTANK MAX VALUE
			"15",				//WEIGHT
			"2",				//CARGOSIZE
			"1",				//ROPE LEVEL
			"5",				//ROPE DEFAULT VALUE
			"15",				//ROPE MAX VALUE
			"100",				//MAXHEALTH / DURABILITY
			"100",				//MAX TILT SPEED
			"75"				//MAX TILT VALUE
		}
	};

	// Use this for initialization
	void Start () {
		if (!PlayerPrefs.HasKey("First")) {
			save ();
		} else {
			load ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnLevelWasLoaded(int level) {
        if (level == 1 && showLevelEnd) {
            GameObject.Find("LevelEnd").GetComponent<LevelEndManager>().UpdateLevelEnd(this);
            SaveLoad.SaveWallet(wallet);

        }
    }

	public void save () {
		PlayerPrefs.SetInt("First", 1);
		PlayerPrefs.SetString("Name", playerName);
		PlayerPrefs.SetInt("Platform", playerPlatform);

		//Currently selected copter
		PlayerPrefs.SetInt("Copter", currentCopter);

		//CopterLevels and unlocks
		for (int i = 0; i < copterAmount; i++) {
			PlayerPrefs.SetInt("Copter"+i+"Unlocked", int.Parse(copters[i,3]));
			PlayerPrefs.SetInt("Copter"+i+"Enginelevel", int.Parse(copters[i,4]));
			PlayerPrefs.SetInt("Copter"+i+"Fueltanklevel", int.Parse(copters[i,5]));
			PlayerPrefs.SetInt("Copter"+i+"Ropelevel", int.Parse(copters[i,12]));
		}
	}

	public void load () {
		playerFirst = PlayerPrefs.GetInt("First", playerFirst);
		playerName = PlayerPrefs.GetString("Name", playerName);
		playerStars = PlayerPrefs.GetInt("Stars", playerStars);
		playerCoins = PlayerPrefs.GetInt("Coins", playerCoins);
		playerPlatform = PlayerPrefs.GetInt("Platform", playerPlatform);

		//Currently selected copter
		currentCopter = PlayerPrefs.GetInt("Copter", currentCopter);

		//CopterLevels and unlocks
		for (int i = 0; i < copterAmount; i++) {
			copters[i,3] = PlayerPrefs.GetInt("Copter"+i+"Unlocked").ToString();
			copters[i,4] = wallet.UpgradeLevel("Engine").ToString();
			copters[i,5] = wallet.UpgradeLevel("Fuel").ToString();
			copters[i,12] = wallet.UpgradeLevel("Rope").ToString();
		}
	}

	public void sendLevelEndInfo (int stars, int coins) {
		lastCoins = coins;
		lastStars = stars;
	}
	public int getAndNullLastLevelCoins () {
		int tempCoins = lastCoins;
		lastCoins = 0;
		return tempCoins;
	}
	public int getAndNullLastLevelStars () {
		int tempStars = lastStars;
		lastStars = 0;
		return tempStars;
	}

	public string getName () {
		return playerName;
	}
	public void setName (string name) {
		playerName = name;
		save ();
	}

	public int getStars () {
		return playerStars;
	}
	private void setStars (int stars) {
		playerStars = stars;
		save ();
	}
	public void addStars (int stars) {
		setStars (getStars() + stars);
	}

	public int getCoins () {
		return playerCoins;
	}
	private void setCoins (int coins) {
		playerCoins = coins;
	}
	public void addCoins (int coins) {
		setCoins (getCoins() + coins);
		save ();
	}

	public int getPlatformLevel () {
		return playerPlatform;
	}

	public string[,] getCopters () {
		return copters;
	}
	public int getCopterAmount () {
		return copterAmount;
	}

	public int getCurrentCopter () {
		return currentCopter;
	}
	public void setCurrentCopter (int copter){
		currentCopter = copter;
		save ();
	}
	public void swapCopter () {
		if (currentCopter < getCopterAmount()-1) setCurrentCopter(currentCopter+1);
		else setCurrentCopter(0);
	}

	public void upgradeCurrentEngine () {
        if (int.Parse(copters[currentCopter, 4]) < 10 && wallet.BuyUpgrade("Engine"))
            copters[currentCopter, 4] = wallet.UpgradeLevel("Engine").ToString();
		//save ();
	}

	public void upgradeCurrentFuelTank () {
		if (int.Parse(copters[currentCopter, 5]) < 10 && wallet.BuyUpgrade("Fuel"))
			copters[currentCopter, 5] = wallet.UpgradeLevel("Fuel").ToString();
        //save ();
	}

	public void upgradeCurrentRope () {
		if (int.Parse(copters[currentCopter, 12]) < 10 && wallet.BuyUpgrade("Rope"))
			copters[currentCopter, 12] = wallet.UpgradeLevel("Rope").ToString();
        //save ();
	}
	
	public void resetUpgrades () {
		copters[currentCopter, 4] = "1";
		copters[currentCopter, 5] = "1";
		copters[currentCopter, 12] = "1";
		playerPlatform = 1;
		save ();
	}

    public Upgrade GetUpgrade(string upgrade) {
        return wallet.GetUpgrade(upgrade);
    }

    public bool BuyUpgrade(string upgrade) {
        bool bought = wallet.BuyUpgrade(upgrade);

        return bought;
    }

	//CAREFUL!
	public void resetData () {
		PlayerPrefs.DeleteKey("First");
		PlayerPrefs.DeleteKey("Name");
		PlayerPrefs.DeleteKey("Stars");
		PlayerPrefs.DeleteKey("Coins");
		PlayerPrefs.DeleteKey("Platform");
		PlayerPrefs.DeleteKey("Copter");
		for (int i = 0; i < copterAmount; i++) {
			PlayerPrefs.DeleteKey("Copter"+i+"Unlocked");
			PlayerPrefs.DeleteKey("Copter"+i+"Enginelevel");
			PlayerPrefs.DeleteKey("Copter"+i+"Fueltanklevel");
			PlayerPrefs.DeleteKey("Copter"+i+"Ropelevel");
		}
		Application.LoadLevel("MainMenu");
	}

	
	public void startGame (string levelName) {
		//Debug.Log("Engine Level: " + copters[currentCopter,4] + " Fuel Level: " + copters[currentCopter,5] + " Platform Level: " + playerPlatform);
		//Debug.Log("Engine Level: " + PlayerPrefs.GetInt("Copter"+currentCopter+"Enginelevel").ToString() + " Fuel Level: " + PlayerPrefs.GetInt("Copter"+currentCopter+"Fueltanklevel").ToString() + " Platform Level: " + playerPlatform);
		save ();
		Application.LoadLevel(levelName);
	}

	public void loadMainMenu(bool showLevelEnd, LevelEndInfo end, int menu = 0) {
		this.showLevelEnd = showLevelEnd;
		levelEnd = end;
        if (showLevelEnd) {
            levelEnd.index = int.Parse(Application.loadedLevelName[Application.loadedLevelName.Length - 1].ToString());
            Debug.Log(levelEnd.index);
        }
		currentMenu = menu;
		Application.LoadLevel("MainMenu");
	}

    private void CheckEarnedStars() {

    }
}
