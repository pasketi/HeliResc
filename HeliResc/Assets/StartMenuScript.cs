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
        if (PlayerPrefs.HasKey(SaveStrings.sAutoHoover)) autoHoover = PlayerPrefsExt.GetBool(SaveStrings.sAutoHoover);        
        else { 
            autoHoover = true;
            PlayerPrefsExt.SetBool(SaveStrings.sAutoHoover, autoHoover);
        }

		//Set music on or off
		if (PlayerPrefs.HasKey (SaveStrings.sMusic))
			music = PlayerPrefsExt.GetBool (SaveStrings.sMusic);
		else {
			music = true;
			PlayerPrefsExt.SetBool(SaveStrings.sMusic, music);
		}

		//Set sounds on or off
		if (PlayerPrefs.HasKey (SaveStrings.sSounds))
			sounds = PlayerPrefsExt.GetBool (SaveStrings.sSounds);
		else {
			sounds = true;
			PlayerPrefsExt.SetBool(SaveStrings.sSounds, sounds);
		}

		autoHooverOff.SetActive (!autoHoover);
		soundOff.SetActive (!sounds);
		musicOff.SetActive (!music);
    }

    public void AutoHoover() {
        autoHoover = !autoHoover;
		autoHooverOff.SetActive (!autoHoover);
        PlayerPrefsExt.SetBool(SaveStrings.sAutoHoover, autoHoover);
    }
	public void Sounds() {
		//TODO when the sounds are implemented

		sounds = !sounds;
		soundOff.SetActive (!sounds);
		PlayerPrefsExt.SetBool(SaveStrings.sSounds, sounds);
	}
	public void Music() {
		//TODO when the sounds are implemented

		music = !music;
		musicOff.SetActive (!music);
		PlayerPrefsExt.SetBool(SaveStrings.sMusic, music);
	}
}
