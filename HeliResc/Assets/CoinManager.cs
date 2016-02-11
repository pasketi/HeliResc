using UnityEngine;
using System.Collections;

public class CoinManager : MonoBehaviour {

	private Vector2 coinDestination;
	
	// Use this for initialization
	void Start () {
		coinDestination = (Vector2)Camera.main.ScreenToWorldPoint(GameObject.Find("PanelMoney").GetComponent<RectTransform>().position);
	}
	
	// Update is called once per frame
	void Update () {
		//gameObject.transform.localScale = new Vector3(20f,20f,1f);
		Vector2 force, coinPosition = (Vector2)Camera.main.ScreenToWorldPoint(gameObject.GetComponent<RectTransform>().position);
		force = coinPosition - coinDestination;
		force.Normalize();
		gameObject.GetComponent<Rigidbody2D>().AddForce(force * -500f, ForceMode2D.Force);
	}

	void OnTriggerEnter2D (Collider2D other) {
			if (other.transform.name == "PanelMoney") Destroy (gameObject);
	}
}
