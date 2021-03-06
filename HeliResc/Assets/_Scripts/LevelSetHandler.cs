﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSetHandler : MonoBehaviour {

    public GameObject lockedPanel;
	public GameObject starsPanel;
    public GameObject buttonPrefab; //Prefab of a button to open levels
    public Text starText;
    private GameObject flashOfWhite;
    public Image setImage;          //The image UI-component in the middle of the button set
	public Image setBG, setChest;
	public Sprite setBGDefault, setBGCompleted;
    public string setName;

    public bool Unlocked { 
        get {
            int playerStars = PlayerPrefs.GetInt(SaveStrings.sPlayerStars);
            return set.unlocked && (playerStars >= set.neededStars);
        } 
    }

    public LevelSet Set { get { return set; } }
    private LevelSet set; //The kind of set the group has

    void Awake() {

		flashOfWhite = GameObject.Find("flashOfWhite");
        RectTransform rect = GetComponent<RectTransform>();
        //rect.anchoredPosition *= GameObject.FindObjectOfType<LevelMapScript>().size;

		set = LevelHandler.GetLevelSet(setName);
		if (set.checkCompletion() && !set.completed) setChest.enabled = true;

		int playerStars = PlayerPrefs.GetInt(SaveStrings.sPlayerStars);
		if (playerStars >= set.neededStars && set.unlocked == true && set.animated == true && set.completed == true)
			SetCompleted();
		else if (playerStars >= set.neededStars && set.unlocked == true && set.animated == true) {
			SetUnlocked();
		} else
			SetLocked(playerStars);
    }

    private void SetLocked(int playerStars) {
		LevelSet lastSet = LevelHandler.GetLevelSet(set.setIndex - 1);

        lockedPanel.SetActive(true);
		if (!set.unlocked && !lastSet.unlocked)
			starsPanel.SetActive(false);
		else starsPanel.SetActive(true);
        setImage.enabled = false;
		setBG.sprite = setBGDefault;

        starText.text = playerStars + "/" + set.neededStars;
    }

    private void SetUnlocked() {

        setImage.enabled = true;
        lockedPanel.SetActive(false);
		setBG.sprite = setBGDefault;
        
		float mapSize = GameObject.FindObjectOfType<LevelMapScript> ().size;
		for (int i = 0; i < set.levelAmount; i++) {
            GameObject go = Instantiate(buttonPrefab) as GameObject;
            go.transform.SetParent(transform);
            LevelButtonHandler handler = go.GetComponent<LevelButtonHandler>();
            handler.Init(i, set);

            handler.SetPosition(CalculateButtonPosition(i, mapSize));
        }
        if (set.setImage == null)
            setImage.enabled = false;
        else
            setImage.sprite = set.setImage;
		
		if (set.checkCompletion() && !set.completed) setChest.enabled = true;
    }    

	private void SetCompleted () {
		SetUnlocked();
		setBG.sprite = setBGCompleted;
	}

    public void OpenSetFirstTime() {
        set.animated = true;
        set.Save();
        StartCoroutine(UnlockFirstTime());
    }

    private IEnumerator UnlockFirstTime() {
        setImage.enabled = true;
        lockedPanel.SetActive(false);

        float mapSize = GameObject.FindObjectOfType<LevelMapScript>().size;
        
        for (int i = 0; i < set.levelAmount; i++)
        {
            GameObject go = Instantiate(buttonPrefab) as GameObject;
            go.transform.SetParent(transform);
            LevelButtonHandler handler = go.GetComponent<LevelButtonHandler>();
            handler.Init(i, set);

            StartCoroutine(handler.SetPositionAnimated(CalculateButtonPosition(i, mapSize)));
        }
        if (set.setImage == null)
            setImage.enabled = false;
        else
            setImage.sprite = set.setImage;
        yield return null;

    }

	public void completeSet () {
		flashOfWhiteManager flash;
		flash = flashOfWhite.GetComponent<flashOfWhiteManager>();
		if (flash.setLevelSet(gameObject)) {
			flash.playAnimation();
			set.completed = true;
			set.Save();
		} else Debug.LogError("LevelSet failed!");
	}

    private Vector3 CalculateButtonPosition(int index, float mapSize) {
        Vector3 vec = new Vector2();
        float angle = (360f / set.levelAmount) * index * Mathf.Deg2Rad;

        float x = Mathf.Sin(angle);
        float y = Mathf.Cos(angle);

        vec.x = x;
        vec.y = y;
        float height = 0;
        RectTransform rect = GetComponent<RectTransform>();

        //Depending on the aspect ratio set the circle's radius from the rectangle's width or height
        float h = rect.sizeDelta.y; //((rect.anchorMax.y - rect.anchorMin.y) * Screen.height);
        float w = rect.sizeDelta.x; //((rect.anchorMax.x - rect.anchorMin.x) * Screen.width);
        if (h <= w)
        {
            height = (h * 0.5f) * .99451f;
        }
        else
        {
            height = (w * 0.5f) * .99451f;
        }


        vec *= (height);
        return vec;
        //return transform.position + vec;
    }
}

[System.Serializable]
public class LevelSet {   

	public string levelSetName;     //The identifier of the set. Crate, swimmer etc.
	public int levelAmount;         //how many levels is in the set
	public int completionReward;	//How many coins you'll get for completing
	public Sprite setImage;         //The image to show in the middle of the set
	public int neededStars;			//How many stars is required to open the set
    public int setIndex;            //the index of the set in the list of level sets
    public bool unlocked;           //Is the set unlocked
    public bool animated;           //Has the animation been played
	public bool completed;			//Completed and animated

	public Objective objective1;
	public Objective objective2;
	public Objective objective3;
    public string challenge1;
    public string challenge2;
    public string challenge3;               //Description of the level challenges 1, 2 and 3
    public float[] levelTimeChallenges;     //Array that contains the time limits for each level in the set

    public void Save() {
        PlayerPrefsExt.SetBool(levelSetName + "Set", unlocked);
        PlayerPrefsExt.SetBool(levelSetName + "Animation", animated);
		PlayerPrefsExt.SetBool(levelSetName + "Completed", completed);
    }
    public void Load() {
        unlocked = PlayerPrefsExt.GetBool(levelSetName + "Set") || unlocked;
        animated = PlayerPrefsExt.GetBool(levelSetName + "Animation") || animated;
		completed = PlayerPrefsExt.GetBool(levelSetName + "Completed") || completed;
    }

	public bool checkCompletion () {
		List<Level> temp = new List<Level>();
		bool result = true;

		for (int i = 0; i < levelAmount; i++) {
			temp.Add(Level.Load(levelSetName, i));
		}
			
		foreach (Level lev in temp) {
			if (result) result = lev.checkLevelCompletion(lev);
		}

		return result;
	}
}