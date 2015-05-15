using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStoryScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Button b = GetComponent<Button>();
        b.onClick.AddListener(() => PressPlay());	    
	}

    private void PressPlay() {
		PlayerPrefs.SetInt ("levelToLoad", 1);
        Application.LoadLevel("IntroScreen");
    }
}
