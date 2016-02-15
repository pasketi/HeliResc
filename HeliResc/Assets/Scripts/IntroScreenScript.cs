using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroScreenScript : MonoBehaviour {

    public Intro[] intro;
    public Text[] challenges;

	public Text introText;
    public Image introComic;

	private int spriteIndex = 0;
	
	public GameObject loadImage;

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
		spriteIndex = 0;

        if (i.showText == true) {
            introText.enabled = true;
            introComic.enabled = false;
            introText.text = i.text;

        }
        else {
            introText.enabled = false;
            introComic.enabled = true;
			introComic.sprite = i.sprite[spriteIndex];
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
		LevelSet set = LevelHandler.GetLevelSet();
		Intro i = intro[set.setIndex];
		
		if (spriteIndex >= i.sprite.Length-1) {
			loadImage.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
			GameManager.LoadLevel (LevelHandler.CurrentLevel.name);
		} else {
			spriteIndex++;
			introComic.sprite = i.sprite[spriteIndex];
		}
	}

	public void PressBack() {
        GameManager.LoadLevel("LevelMap");
	}

    [System.Serializable]
    public class Intro {
        public bool showText;
        public Sprite[] sprite;
        public string text;
    }
}
