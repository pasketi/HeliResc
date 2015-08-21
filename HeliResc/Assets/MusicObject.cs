using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicObject : MonoBehaviour
{

    public AudioClip clips;           //References to all the clips the object can play    
    public bool loop = true;

    private AudioSource _audio;

    // Use this for initialization
    void Start() {
        _audio = GetComponent<AudioSource>();
        Mute(SoundMusic.MusicMuted);

        _audio.loop = loop;

        if (clips == null)
            Debug.LogError("audio clip not set");

        AudioClip c = clips;
        _audio.clip = c;
        PlayMusic();
        
    }   

    public void Mute(bool mute) {
        if (_audio == null)
            _audio = GetComponent<AudioSource>();
        _audio.mute = mute;
    }

    public void PlayMusic()
    {        
        _audio.Play();
    }
}