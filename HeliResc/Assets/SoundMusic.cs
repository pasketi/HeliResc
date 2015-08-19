using UnityEngine;
using System.Collections;

public class SoundMusic : MonoBehaviour {

    public static bool SoundMuted { get { return instance.soundMuted; } }
    public static bool MusicMuted { get { return instance.musicMuted; } }

	private static SoundMusic instance;
    private bool soundMuted;
    private bool musicMuted;

    void OnLevelWasLoaded(int level) {        
        MuteSoundObjects(soundMuted);
        MuteMusicObjects(musicMuted);
    }

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

    private void MuteMusicObjects(bool mute) {
        MonoBehaviour[] objects = GameObject.FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour mono in objects) {
            MusicObject mo = mono as MusicObject;
            if (mo != null)
                mo.Mute(mute);
        }
    }

    private void MuteSoundObjects(bool mute) {
        MonoBehaviour[] objects = GameObject.FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour mono in objects) {
            SoundObject so = mono as SoundObject;
            if (so != null)
                so.Mute(mute);
        }
    }    

	public static void MuteMusic(bool mute) {
        instance.musicMuted = mute;
        instance.MuteMusicObjects(mute);
	}
    public static void MuteSounds(bool mute) {
        instance.soundMuted = mute;
        instance.MuteSoundObjects(mute);
    }	
}

public interface MusicObject {
    void Mute(bool mute);
    void PlaySound(bool loop);
}

public interface SoundObject {
    void Mute(bool mute);
    void PlaySound();
}