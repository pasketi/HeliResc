using UnityEngine;
using System.Collections;

public class SoundMusic : MonoBehaviour {

    public static bool SoundMuted { get { return instance.soundMuted; } }
    public static bool MusicMuted { get { return instance.musicMuted; } }

	private static SoundMusic instance;
    private bool soundMuted;
    private bool musicMuted;    

    void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);

        //Set music on or off
        if (PlayerPrefs.HasKey(SaveStrings.sMusic))
            musicMuted = PlayerPrefsExt.GetBool(SaveStrings.sMusic);
        else
        {
            musicMuted = false;
            PlayerPrefsExt.SetBool(SaveStrings.sMusic, musicMuted);
        }

        //Set sounds on or off
        if (PlayerPrefs.HasKey(SaveStrings.sSounds))
            soundMuted = PlayerPrefsExt.GetBool(SaveStrings.sSounds);
        else
        {
            soundMuted = false;
            PlayerPrefsExt.SetBool(SaveStrings.sSounds, soundMuted);
        }
    }

	public static void MuteMusic(bool mute) {
        instance.musicMuted = mute;
        PlayerPrefsExt.SetBool(SaveStrings.sMusic, mute);
	}
    public static void MuteSounds(bool mute) {
        instance.soundMuted = mute;
        PlayerPrefsExt.SetBool(SaveStrings.sSounds, mute);
    }	
}