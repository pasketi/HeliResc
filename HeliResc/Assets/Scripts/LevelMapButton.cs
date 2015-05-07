﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelMapButton : MonoBehaviour {

    //Index of the level. First is 1 etc
	public int levelIndex;

    //Determines if the level can be entered
	public bool levelLocked;

    //DELETE ?
	public string levelName;

    //The star images on top of button
	public Image[] stars;

    //Sprites to assign to the images
	public Sprite starUnlocked;
	public Sprite starLocked;

    //Contain all info of the level to be saved
    private LevelInfo levelInfo;

    //DELETE?
	[Range(0, 3)]
	public int starsAwarded;

    //The sprites on the button
    public Sprite buttonLocked;
    public Sprite buttonUnlocked;

    //Changes depending if the level is locked
    public Image buttonImage;

    //The text in the middle of the button
	public Text buttonText;

	// Use this for initialization
	void Start () {        
        levelInfo = SaveLoad.LoadLevelInfo(levelIndex);

        Debug.Log("Level " + levelIndex + " is locked: " + levelInfo.locked);

        stars[0].sprite = levelInfo.star1 ? starUnlocked : starLocked;
        stars[1].sprite = levelInfo.star2 ? starUnlocked : starLocked;
        stars[2].sprite = levelInfo.star3 ? starUnlocked : starLocked;

        for (int i = 0; i < stars.Length; i++) {
            stars[i].enabled = !levelInfo.locked;         
        }

        if (levelInfo.locked)
        {
            buttonImage.sprite = buttonLocked;
            buttonText.text = "";
        }
        else {
            buttonImage.sprite = buttonUnlocked;
            buttonText.text = levelIndex.ToString();
        }
		levelLocked = levelInfo.locked;
		
	}

	public void StartLevel() {
		if(!levelLocked) 
			Application.LoadLevel("Level" + levelIndex.ToString());
	}
}
