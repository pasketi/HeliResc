using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStoryScript : MonoBehaviour {

    void OnEnable() {
        EventManager.StartListening(SaveStrings.eEscape, Application.Quit);   
    }
    void OnDisable() {
        EventManager.StopListening(SaveStrings.eEscape, Application.Quit);
    }

	// Use this for initialization
	void Awake () {
        Button b = GetComponent<Button>();
        b.onClick.AddListener(() => PressPlay());	    
	}

    private void PressPlay() {
        GameManager.LoadLevel("IntroScreen");
    }
}
