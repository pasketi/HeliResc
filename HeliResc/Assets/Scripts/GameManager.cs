using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private bool showLevelEnd = false;
	public bool ShowLevelEnd { get { return showLevelEnd; } }

    public Wallet wallet;
	public LevelEndInfo levelEnd;

	private int currentMenu = 0;
	public int CurrentMenu { get { return currentMenu; } }

    public GameObject[] copters;
    public Dictionary<string, Copter> CopterScripts;
    public GameObject CurrentCopter { get { return copters[CurrentCopterIndex]; } }
    public Copter CurrentCopterScript { get { return CopterScripts[CurrentCopter.name]; } }
    public int CurrentCopterIndex { get { return Mathf.Clamp(currentCopter, 0, copters.Length - 1); } }


	void Awake(){
		DontDestroyOnLoad(gameObject);
        wallet = SaveLoad.LoadWallet();
	}

	private const int copterAmount = 2; //FOR EVERY NEW COPTER, ADD 1 HERE
    public int CopterAmount { get { return copterAmount; } }

	private string 	playerName = "Anonymous";
	private int		playerFirst = 0,
					playerStars = 0, 
					playerCoins = 0, 
					playerPlatform = 1,
					currentCopter = 1,  //0 == default, 1 = watercopter
					lastStars = 0,
					lastCoins = 0;	
    
	// Use this for initialization
	void Start () {
              
        
        CopterScripts = new Dictionary<string,Copter>();
        foreach(GameObject go in copters){
            Copter c = Instantiate(go).GetComponent<Copter>();            
            CopterScripts.Add(go.name, c);
            c.Disable();
        }
     
        if (!PlayerPrefs.HasKey("First")) {
            save();
        } else {
            load();
        }
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Escape))
            wallet.AddMoney(1);
	}

    void OnLevelWasLoaded(int level) {
        if (level == 1 && !PlayerPrefs.HasKey("FirstLogin")) {
            PlayerPrefsExt.SetBool("FirstLogin", true);
            Application.LoadLevel("GameStory");
            return;
        }
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
            //copters[i,3] = PlayerPrefs.GetInt("Copter"+i+"Unlocked").ToString();
            //copters[i,4] = wallet.UpgradeLevel("Engine").ToString();
            //copters[i,5] = wallet.UpgradeLevel("Fuel").ToString();
            //copters[i,12] = wallet.UpgradeLevel("Rope").ToString();
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
        return null;
        //return copters;
    }
	public int getCopterAmount () {
		return copterAmount;
	}

    //public int getCurrentCopter () {
    //    return currentCopter;
    //}
	public void setCurrentCopter (int copter){
		currentCopter = copter;
		save ();
	}
    //public void swapCopter () {
    //    if (currentCopter < getCopterAmount()-1) setCurrentCopter(currentCopter+1);
    //    else setCurrentCopter(0);
    //}

	
	public void resetUpgrades () {
        //copters[currentCopter, 4] = "1";
        //copters[currentCopter, 5] = "1";
        //copters[currentCopter, 12] = "1";
		playerPlatform = 1;
		save ();
	}        

    public bool BuyUpgrade(Upgrade upgrade) {
        //TODO

        return false;
    }

	public void BuyCopter(int index) {
		//TODO
	}

	//CAREFUL!
	public void resetData () {
        PlayerPrefs.DeleteAll();        
        Application.Quit();
	}

	
	public void startGame (string levelName) {		
		save ();
		Application.LoadLevel(levelName);
	}

    public void GameOver() { }

	public void loadMainMenu(bool showLevelEnd, LevelEndInfo end = null, int menu = 0) {
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
