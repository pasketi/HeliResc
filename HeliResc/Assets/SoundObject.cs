using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundObject : MonoBehaviour {

    public AudioClip[] clips;           //References to all the clips the object can play
    public bool playSoundOnTrigger;
    public bool playSoundOnCollision;

    public bool loopable;

    private AudioSource _audio;

	// Use this for initialization
	void Start () {
        _audio = GetComponent<AudioSource>();
        Mute(SoundMusic.SoundMuted);

        _audio.loop = loopable;
        if (loopable == true)
        {
            _audio.clip = clips[0];
            _audio.Play();
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (playSoundOnTrigger == true) {
            if (other.CompareTag("Copter") || other.CompareTag("Hook")) {
                PlaySound();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (playSoundOnCollision == true) {
            if (collision.gameObject.CompareTag("Copter") || collision.gameObject.CompareTag("Hook")) {
                PlaySound();
            }
        }
    }

    public void Mute(bool mute)
    {
        if (_audio == null)
            _audio = GetComponent<AudioSource>();
        _audio.mute = mute;
    }

    public void PlaySound()
    {
        if (clips == null || clips.Length <= 0)
            Debug.LogError("audio clips not set");
        if (_audio == null)
            _audio = GetComponent<AudioSource>();

        AudioClip c = clips[Random.Range(0, clips.Length)];
        _audio.PlayOneShot(c);
    }
    public void PlayUsingHandler(AudioClip clip) {
        SoundMusic.PlaySound(clip);
    }
}
