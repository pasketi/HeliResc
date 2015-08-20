using UnityEngine;
using System.Collections;

public class CopterCrashEffect : MonoBehaviour, SoundObject {

    private AudioSource _audio;

	// Use this for initialization
	void Start () {
        _audio = GetComponent<AudioSource>();
	}

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Tag" + collision.gameObject.tag);
        if (collision.gameObject.tag == "Copter") {
            PlaySound();
        }
    }

    public void Mute(bool mute)
    {
        if (_audio == null)
            _audio = GetComponent<AudioSource>();
        _audio.volume = mute ? 1 : 0;
    }

    public void PlaySound()
    {
        Debug.Log("Sound");
        _audio.Play();
    }
}
