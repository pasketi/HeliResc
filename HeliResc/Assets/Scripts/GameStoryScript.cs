using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStoryScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Button b = GetComponent<Button>();
        GameManager g = GameObject.FindObjectOfType<GameManager>();
        b.onClick.AddListener(() => PressPlay());	    
	}

    private void PressPlay() {
        Application.LoadLevel("Level1");
    }
}
