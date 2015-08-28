using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SetReset : MonoBehaviour {

    public Button resetButton;

	// Use this for initialization
	void Start () {
		resetButton.onClick.AddListener(() => GameObject.Find ("GameManager").GetComponent<GameManager>().resetData());
	}

    public void UnlockAll() {
        foreach (List<Level> levels in LevelHandler.Levels.Values) {
            foreach (Level l in levels) {
                l.star1 = true;
                l.star2 = true;
                l.star3 = true;
                l.rubyFound = true;
                l.unlocked = true;                
                Level.Save(l);
            }
        }
        foreach (LevelSet set in LevelHandler.levelSets) {
            set.unlocked = true;
            set.animated = true;
            set.Save();
        }
        PlayerPrefs.SetInt(SaveStrings.sPlayerRubies, LevelHandler.LevelCount);
        PlayerPrefs.SetInt(SaveStrings.sPlayerStars, LevelHandler.LevelCount * 3);
        GameManager.LoadLevel("MainMenu");
    }
}
