using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BurningObject : MonoBehaviour {

	public bool burning;
	public float burnStateTime;				//The time it will take from the object to advance to next level of burning
	public Sprite[] burnSprites;			//The sprites of different states of the burning object. Assign the most healthy to the first slot
	public List<GameObject> fires;			//List of bigger fire gameobjects on the object
	public List<GameObject> smallFires;		//list of the small fire gameobject on the object

	private System.Action UpdateMethod;


	private SpriteRenderer _sprite;			//Reference to sprite renderer component

	private bool ignite;					//Should the object ignite nearby objects
	private int burnState;					//The current state of burning. Maximum value is determined by the lenght of burning sprites array
	private int maxState;					//Max value of the burnstate variable
	private float damage;					//A timer to keep track of how long the object has been burning


	// Use this for initialization
	void Start () {
		_sprite = GetComponent<SpriteRenderer> ();
		_sprite.sprite = burnSprites [0];
		burnState = 1;
		maxState = burnSprites.Length;

		if (burning == true)
			StartBurning ();
		else
			StopBurning ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMethod ();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.CompareTag ("Burn")) {

		}
	}

	public void StartBurning() {
		UpdateMethod = Burn;
		burning = true;
	}
	public void StopBurning() {
		UpdateMethod = () => { };
		burning = false;
	}

	private void Burn() {
		damage += Time.deltaTime;
		if (damage >= burnState * burnStateTime) {
			NextState();
		}
	}
	private void NextState() {
		_sprite.sprite = burnSprites [burnState];
		burnState++;
		if (burnState >= maxState) {
			Die ();
		}
	}
	private void Die() {

	}
}
