using UnityEngine;
using System.Collections;

public class CopterSoundEffect : MonoBehaviour {

    public float minPitch = 0.75f;
    public float maxPitch = 1.5f;
    
    private AudioSource _audio;
    private Copter copter;

    private float power;

	// Use this for initialization
	void Start () {
        copter = GetComponent<Copter>();
        _audio = GetComponent<AudioSource>();
        Mute(SoundMusic.SoundMuted);
	}

    void Update() { 
        _audio.pitch = Mathf.Clamp((maxPitch * copter.engine.CurrentPowerPercentage), minPitch, maxPitch);
    }

    public void Mute(bool mute)
    {
        if (_audio == null)
            _audio = GetComponent<AudioSource>();
        _audio.mute = mute;
    }

    public void PlaySound()
    {
        _audio.Play();
    }
}
