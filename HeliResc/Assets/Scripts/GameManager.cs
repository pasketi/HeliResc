using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public float pixelDragThresholdMultiplier = 20f;
	private bool showLevelEnd = false;
	public bool ShowLevelEnd { get { return showLevelEnd; } }

    public Wallet wallet;
	public LevelEndInfo levelEnd;

    private int currentMenu = 0;                                                                            //The menu that is currently shown or being transitioned to
	public int CurrentMenu { get { return currentMenu; } }                                                  //Getter for the current menu

    public GameObject[] copters;
    public Dictionary<int, CopterInfo> CopterInfos;                                                        	//List of all copters CopterInfo. Access with the index.
    public GameObject CurrentCopter { get { return copters[CurrentCopterIndex]; } }                         //Returns the selected copter prefab
    public int CurrentCopterIndex { get { return Mathf.Clamp(currentCopter, 0, copters.Length - 1); }
									set { currentCopter = Mathf.Clamp(value, 0, copters.Length - 1); } }

	private int currentCopter = 0;
	private int playerStars;
	private int playerCoins;

    private static GameManager instance;


	void Awake() {
        instance = this;
		DontDestroyOnLoad(gameObject);
        wallet = SaveLoad.LoadWallet();
	}

	// Use this for initialization
	void Start () {
              
        
        CopterInfos = new Dictionary<int,CopterInfo>();
		for(int i = 0; i < copters.Length; i++){
            Copter c = Instantiate(copters[i]).GetComponent<Copter>();            
            CopterInfos.Add(i, c.GetCopterInfo(i));
            c.Disable();
        }
     
        if (!PlayerPrefs.HasKey("First")) {
            save();
        } else {
            load();
        }
	}
    void OnDisable() {
        EventManager.StopListening(SaveStrings.eEscape, Application.Quit);
    }
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
            EventManager.TriggerEvent(SaveStrings.eEscape);
	}

    void OnLevelWasLoaded(int level) {
		if (GameObject.Find("LoadImage") != null) GameObject.Find("LoadImage").transform.localScale = new Vector3(1f,0f,1f);
		if (GameObject.Find("EventSystem") != null ) GameObject.Find("EventSystem").GetComponent<EventSystem>().pixelDragThreshold = (int)(pixelDragThresholdMultiplier * ((float)Screen.width / (float)Screen.height));

        //Quit the game when player press back button
        if(level == 1) EventManager.StartListening(SaveStrings.eEscape, Application.Quit);
        else EventManager.StopListening(SaveStrings.eEscape, Application.Quit);

        Time.timeScale = 1;

        if (level == 1 && !PlayerPrefs.HasKey("FirstLogin")) {
            PlayerPrefsExt.SetBool("FirstLogin", true);
            GameManager.LoadLevel("GameStory");
            return;
        }
        if (level == 1 && showLevelEnd) {
            GameObject.Find("LevelEnd").GetComponent<LevelEndManager>().UpdateLevelEnd(this);
            SaveLoad.SaveWallet(wallet);

        }                            
    }

	public void save () {
		PlayerPrefs.SetInt("First", 1);

		//Currently selected copter
		PlayerPrefs.SetInt("Copter", currentCopter);
		
	}

	public void load () {
		//playerFirst = PlayerPrefs.GetInt("First", playerFirst);
		//playerName = PlayerPrefs.GetString("Name", playerName);
		playerStars = PlayerPrefs.GetInt("Stars", playerStars);
		playerCoins = PlayerPrefs.GetInt("Coins", playerCoins);

		//Currently selected copter
		currentCopter = PlayerPrefs.GetInt(SaveStrings.sSelectedCopter, currentCopter);


	}    

	//CAREFUL!
	public void resetData () {
        PlayerPrefs.DeleteAll();        
        Application.Quit();
	}

	
	public void startGame (string levelName) {		
		save ();
        GameManager.LoadLevel(levelName);
	}

	public static void LoadLevel(string level) {
        instance.StartLevel(level);
	}

	public string LevelName () {
		return SceneManager.GetActiveScene().name;
	}

    public void StartLevel(string level) {
        StartCoroutine(LoadLevelAsync(level));
    }
    public IEnumerator LoadLevelAsync(string level) {
        AsyncOperation async = Application.LoadLevelAsync(level);
        yield return async;
        Debug.Log("Loading level " + level + " done");
    }

    public void GameOver() { }

	public void loadMainMenu(bool showLevelEnd, LevelEndInfo end = null, int menu = 0) {
		this.showLevelEnd = showLevelEnd;
		levelEnd = end;
        if (showLevelEnd) {                      
            //levelEnd.index = int.Parse(Application.loadedLevelName[Application.loadedLevelName.Length - 1].ToString());
            //levelEnd.index = PlayerPrefs.GetInt("levelToLoad");

        }
		currentMenu = menu;
		GameManager.LoadLevel("MainMenu");
	}
	public Dictionary<int, CopterInfo> GetCopterInfo() {
		foreach (CopterInfo i in CopterInfos.Values) {
			i.Load();
		}
		return CopterInfos;
	}

    private void CheckEarnedStars() {

    }

	/*private IEnumerator Loader () {
				for (int curLvl = 0; curLvl < SceneManager.sceneCount; curLvl++) SceneManager.LoadSceneAsync(curLvl);
				yield return null;
	}*/
}