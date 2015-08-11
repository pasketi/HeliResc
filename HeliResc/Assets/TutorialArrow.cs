using UnityEngine;
using System.Collections;

public class TutorialArrow : MonoBehaviour {

	public float time = 1.5f;
	private SpriteRenderer _sprite;

	// Use this for initialization
	void Start () {
		_sprite = GetComponentInChildren<SpriteRenderer> ();

	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals ("Copter")) {
			StartCoroutine(Tutorial.FadeOut (_sprite, time));
		}
	}
}
