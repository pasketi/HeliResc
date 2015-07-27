using UnityEngine;
using System.Collections;

public class StartMenuScript : MonoBehaviour {
	    
	public GameObject autoHooverOff;
	public GameObject soundOff;
	public GameObject musicOff;

    private bool autoHoover;
	private bool sounds;
	private bool music;

    void Start() {
		//Set autohoover on or off
        if (PlayerPrefs.HasKey(SaveStrings.autoHoover)) autoHoover = PlayerPrefsExt.GetBool(SaveStrings.autoHoover);        
        else { 
            autoHoover = true;
            PlayerPrefsExt.SetBool(SaveStrings.autoHoover, autoHoover);
        }

		//Set music on or off
		if (PlayerPrefs.HasKey (SaveStrings.music))
			music = PlayerPrefsExt.GetBool (SaveStrings.music);
		else {
			music = true;
			PlayerPrefsExt.SetBool(SaveStrings.music, music);
		}

		//Set sounds on or off
		if (PlayerPrefs.HasKey (SaveStrings.sounds))
			sounds = PlayerPrefsExt.GetBool (SaveStrings.sounds);
		else {
			sounds = true;
			PlayerPrefsExt.SetBool(SaveStrings.sounds, sounds);
		}

		autoHooverOff.SetActive (!autoHoover);
		soundOff.SetActive (!sounds);
		musicOff.SetActive (!music);
    }

    public void AutoHoover() {
        autoHoover = !autoHoover;
		autoHooverOff.SetActive (!autoHoover);
        PlayerPrefsExt.SetBool(SaveStrings.autoHoover, autoHoover);
    }
	public void Sounds() {
		//TODO when the sounds are implemented

		sounds = !sounds;
		soundOff.SetActive (!sounds);
		PlayerPrefsExt.SetBool(SaveStrings.sounds, sounds);
	}
	public void Music() {
		//TODO when the sounds are implemented

		music = !music;
		musicOff.SetActive (!music);
		PlayerPrefsExt.SetBool(SaveStrings.music, music);
	}
}
