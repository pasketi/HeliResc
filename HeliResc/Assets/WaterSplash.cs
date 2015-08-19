using UnityEngine;
using System.Collections;

public class WaterSplash : MonoBehaviour, SoundObject {

    private AudioSource _audio;

	// Use this for initialization
	void Awake () {
        _audio = GetComponent<AudioSource>();
        Mute(SoundMusic.SoundMuted);
	}

    public void Mute(bool mute)
    {
        if (_audio == null)
            _audio = GetComponent<AudioSource>();
        _audio.volume = mute ? 1 : 0;
    }

    public void PlaySound()
    {
        _audio.Play();
    }
}
