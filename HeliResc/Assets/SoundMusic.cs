using UnityEngine;
using System.Collections;

public class SoundMusic : MonoBehaviour {

    public static bool SoundMuted { get { return instance.soundMuted; } }
    public static bool MusicMuted { get { return instance.musicMuted; } }

    public AudioSource soundSource;
    public AudioSource musicSource;

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

    public static void PlaySound(AudioClip clip) {
        if(instance.soundMuted == false)
            instance.soundSource.PlayOneShot(clip);
    }
    public static void PlayMusic(AudioClip clip) {
        if (instance.musicMuted == false) {
            instance.musicSource.clip = clip;
            instance.musicSource.Play();
        }
    }
    public static void Stop() {
        instance.soundSource.Stop();
        instance.musicSource.Stop();
    }
}