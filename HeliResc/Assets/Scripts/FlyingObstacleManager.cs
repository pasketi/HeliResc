using UnityEngine;
using System.Collections;

public class FlyingObstacleManager : MonoBehaviour {

	private LevelManager manager;
	private float tempPatrol = 0f;
	public float speed = 0.2f, patrolTime = 3f;
	public bool movingRight = true, patrolling = true;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("LevelManagerO").GetComponent<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (patrolling) {
			tempPatrol += Time.deltaTime;
			if (tempPatrol >= patrolTime) {
				tempPatrol = 0f;
				movingRight = !movingRight;
			}
		}

		if (movingRight && !manager.getPaused()) {
			gameObject.transform.localScale = new Vector3(1f, 1f);
			gameObject.transform.Translate(new Vector3(speed * Time.deltaTime, 0f));
		} else if (!movingRight && !manager.getPaused()) {
			gameObject.transform.localScale = new Vector3(-1f, 1f);
			gameObject.transform.Translate(new Vector3(-speed * Time.deltaTime, 0f));
		}
	}
}
