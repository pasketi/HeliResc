using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroScreenScript : MonoBehaviour {

	public string[] introTexts;
	public Text introText;

	private int levelToLoad = 1;

    void OnEnable() {
        EventManager.StartListening(SaveStrings.escape, PressBack);
    }
    void OnDisable() {
        EventManager.StopListening(SaveStrings.escape, PressBack);
    }

	// Use this for initialization
	void Start () {
		if(PlayerPrefs.HasKey("levelToLoad"))
			levelToLoad = PlayerPrefs.GetInt ("levelToLoad");
		introText.text = introTexts [levelToLoad - 1];
	}	
	
	public void PressPlay() {
		Application.LoadLevel ("Level" + levelToLoad.ToString());
	}

	public void PressBack() {		
    	Application.LoadLevel ("LevelMap");
	}
}
