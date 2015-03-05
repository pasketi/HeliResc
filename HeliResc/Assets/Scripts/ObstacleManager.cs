using UnityEngine;
using System.Collections;

public class ObstacleManager : MonoBehaviour {
	
	public bool instaKill = false;
	public float damageMultiplier = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.transform.tag == "Copter") {
			if (!instaKill)
				collision.gameObject.GetComponent<CopterManagerTouch>().changeHealth(-collision.relativeVelocity.magnitude * damageMultiplier);
			else GameObject.Find("LevelManagerO").GetComponent<LevelManager>().levelFailed(1);
		}
	}
}
