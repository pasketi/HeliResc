using System;
using UnityEngine;
using System.Collections;

public class ObstacleManager : MonoBehaviour {

    public Action<string> ObstacleHit = (string obstacleTag) => { };

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
					collision.gameObject.GetComponent<Copter>().health.TakeDamage(collision.relativeVelocity.magnitude * damageMultiplier);
				} else {
                    collision.gameObject.GetComponent<Copter>().health.TakeDamage(fixedDamageAmount);
				}
			} else GameObject.Find("LevelManagerO").GetComponent<LevelManager>().levelFailed(1);

			if (diesOnContact && deathAnimation != null) 
				Instantiate (deathAnimation, transform.position, Quaternion.identity);
			if (diesOnContact) Destroy(gameObject);
		}

		if (collision.gameObject.transform.tag == "Hook" || collision.gameObject.transform.tag == "Crate") {
            if (killsHook) collision.gameObject.GetComponent<Copter>().rope.KillHook();

			if (diesOnContact && deathAnimation != null) 
				Instantiate (deathAnimation, transform.position, Quaternion.identity);
			if (diesOnContact) Destroy(gameObject);
		}

        ObstacleHit(gameObject.tag);
    }

    void OnTriggerEnter2D(Collider2D other) {
        
        if (other.gameObject.transform.tag == "Copter")
        {
            if (!instaKill)
            {
                if (fixedDamage)
                {
                    other.gameObject.GetComponent<Copter>().health.TakeDamage(fixedDamageAmount);
                }
            }
            else GameObject.Find("LevelManagerO").GetComponent<LevelManager>().levelFailed(1);

            if (diesOnContact && deathAnimation != null)
                Instantiate(deathAnimation, transform.position, Quaternion.identity);
            if (diesOnContact) Destroy(gameObject);
        }

        ObstacleHit(gameObject.tag);        
    }
}
