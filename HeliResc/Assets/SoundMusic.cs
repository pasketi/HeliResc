using UnityEngine;
using System.Collections;

public class SoundMusic : MonoBehaviour {

	public AudioSource music;
	public AudioSource sounds;

	public AudioClip copterSlam;
	public AudioClip mineExplosion;
	public AudioClip birdExplosion;
	public AudioClip tucanScream;
	public AudioClip waterSplash;

	private static SoundMusic instance;

	// Use this for initialization
	void Start () {
		instance = this;
		DontDestroyOnLoad (gameObject);
	}
	
	public static void MuteMusic(bool mute) {

	}
	public static void MuteSounds(bool mute) {

	}
	public static void CopterSlam() {
		instance.sounds.PlayOneShot (instance.copterSlam);
	}
	public static void MineExplosion() {
		instance.sounds.PlayOneShot (instance.mineExplosion);
	}
	public static void BirdExplosion() {
		instance.sounds.PlayOneShot (instance.birdExplosion);
	}
	public static void TucanScream() {
		instance.sounds.PlayOneShot (instance.tucanScream);
	}
	public static void WaterSplash() {
		instance.sounds.PlayOneShot (instance.waterSplash);
	}
}
