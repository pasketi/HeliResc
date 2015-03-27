using UnityEngine;
using System.Collections;

public class ObstacleManager : MonoBehaviour {

	public GameObject deathAnimation;
	public bool instaKill = false, fixedDamage = false, killsHook = false, diesOnContact = false;
	public float damageMultiplier = 1f, fixedDamageAmount = 20f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.transform.tag == "Copter") {
			if (!instaKill) {
				if (!fixedDamage) { 
					collision.gameObject.GetComponent<CopterManagerTouch>().changeHealth(-collision.relativeVelocity.magnitude * damageMultiplier);
				} else {
					collision.gameObject.GetComponent<CopterManagerTouch>().changeHealth(-fixedDamageAmount);
				}
			} else GameObject.Find("LevelManagerO").GetComponent<LevelManager>().levelFailed(1);
		}

		if (collision.gameObject.transform.tag == "Hook") {
			if (killsHook) GameObject.Find("Copter").GetComponent<CopterManagerTouch>().killHook();
		}

		if (diesOnContact && deathAnimation != null) 
			Instantiate (deathAnimation, transform.position, Quaternion.identity);
		if (diesOnContact) Destroy(gameObject);
	}
}
