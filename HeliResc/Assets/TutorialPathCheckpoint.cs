using UnityEngine;
using System.Collections;

public class TutorialPathCheckpoint : MonoBehaviour {

	public float time = 1.5f;

	private Transform copterTr;
	private Transform _transform;
	private bool active;
	private SpriteRenderer _sprite;

	// Use this for initialization
	void Start () {
		copterTr = GameObject.Find ("Copter").transform;
		_sprite = GetComponent<SpriteRenderer> ();
		_transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (_transform.position.x < copterTr.position.x && active == false) {
			active = true;
			StartCoroutine(Tutorial.FadeOut(_sprite, time));
		}
	}
}
