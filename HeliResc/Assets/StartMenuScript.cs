using UnityEngine;
using System.Collections;

public class StartMenuScript : MonoBehaviour {
	    
	public GameObject autoHooverOff;
	public GameObject soundOff;
	public GameObject musicOff;

    private bool autoHooverOn;
	private bool soundMuted;
	private bool musicMuted;

    void Start() {
		//Set autohoover on or off
        if (PlayerPrefs.HasKey(SaveStrings.sAutoHoover)) autoHooverOn = PlayerPrefsExt.GetBool(SaveStrings.sAutoHoover);        
        else { 
            autoHooverOn = true;
            PlayerPrefsExt.SetBool(SaveStrings.sAutoHoover, autoHooverOn);
        }

        musicMuted = SoundMusic.MusicMuted;
        soundMuted = SoundMusic.SoundMuted;

		autoHooverOff.SetActive (!autoHooverOn);
		soundOff.SetActive (soundMuted);
		musicOff.SetActive (musicMuted);
    }

    public void AutoHoover() {
        autoHooverOn = !autoHooverOn;
		autoHooverOff.SetActive (!autoHooverOn);
        PlayerPrefsExt.SetBool(SaveStrings.sAutoHoover, autoHooverOn);
    }
	public void Sounds() {
		//TODO when the sounds are implemented

        soundMuted = !soundMuted;
        soundOff.SetActive(soundMuted);
        SoundMusic.MuteSounds(soundMuted);
	}
	public void Music() {
		//TODO when the sounds are implemented

		musicMuted = !musicMuted;
		musicOff.SetActive (musicMuted);        //If music is on turn off the cross on the button
        SoundMusic.MuteMusic(musicMuted);
	}
}
