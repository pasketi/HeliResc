using UnityEngine;
using System.Collections;

public class StartMenuScript : MonoBehaviour {
	    
	public GameObject autoHooverOff;
	public GameObject soundOff;
	public GameObject musicOff;
	//public GameObject resetGame;
    
    public GameObject autoHooveDialog;
	public GameObject resetDialog;
    public GameObject credits;

	private GameManager manager;
    private Animator settingsAnimator;
    private bool showSettings;

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

		manager = GameObject.FindObjectOfType<GameManager>();
        settingsAnimator = GetComponent<Animator>();
        showSettings = false;

        musicMuted = SoundMusic.MusicMuted;
        soundMuted = SoundMusic.SoundMuted;
        InitButtons();
    }

    public void InitButtons() {
        autoHooverOff.SetActive(!autoHooverOn);
        soundOff.SetActive(soundMuted);
        musicOff.SetActive(musicMuted);
    }

    public void ShowAutoHooverBox() {
        autoHooveDialog.SetActive(true);
    }

	public void ShowResetBox() {
		resetDialog.SetActive(true);
	}

    public void ShowCredits()
    {
        credits.SetActive(true);
    }

    public void HideCredits()
    {
        credits.SetActive(false);
    }

    public void AutoHoover(bool on) {
        autoHooverOn = on;
		autoHooverOff.SetActive (!autoHooverOn);
        PlayerPrefsExt.SetBool(SaveStrings.sAutoHoover, autoHooverOn);
        autoHooveDialog.SetActive(false);
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

	public void reset(bool yes) {
		resetDialog.SetActive(false);
		if (yes) manager.resetData();
	}

    public void Settings() {
        showSettings = !showSettings;

        if (showSettings == true) settingsAnimator.Play("ShowSettingsAnimation");
        else settingsAnimator.Play("HideSettingsAnimation");
    }
}
