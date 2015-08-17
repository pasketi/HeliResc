using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroScreenScript : MonoBehaviour {

    public Intro[] intro;
    public Text[] challenges;

	public Text introText;
    public Image introComic;

    void OnEnable() {
        EventManager.StartListening(SaveStrings.eEscape, PressBack);
    }
    void OnDisable() {
        EventManager.StopListening(SaveStrings.eEscape, PressBack);
    }

	// Use this for initialization
	void Start () {
        LevelSet set = LevelHandler.GetLevelSet();
        Intro i = intro[set.setIndex];

        if (i.showText == true) {
            introText.enabled = true;
            introComic.enabled = false;
            introText.text = i.text;

        }
        else {
            introText.enabled = false;
            introComic.enabled = true;
            introComic.sprite = i.sprite;
        }

        if (challenges.Length != 3) Debug.LogError("Missing references to challege texts");
        else {
            challenges[0].text = set.challenge1;
            challenges[1].text = set.challenge2;
            challenges[2].text = set.challenge3 + set.levelTimeChallenges[LevelHandler.CurrentLevel.id].ToString() ;
        }
	}

    public void PressPlay()
    {
		GameManager.LoadLevel (LevelHandler.CurrentLevel.name);
	}

	public void PressBack() {		
    	Application.LoadLevel ("LevelMap");
	}

    [System.Serializable]
    public class Intro {
        public bool showText;
        public Sprite sprite;
        public string text;
    }
}
