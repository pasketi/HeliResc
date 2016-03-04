using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CoinManager : MonoBehaviour {

	private Vector2 coinDestination;
	private MoneyUIUpdate moneyText;
	private float scale;
	private float wait = 1f;
	
	// Use this for initialization
	void Start () {
		coinDestination = (Vector2)Camera.main.ScreenToWorldPoint(GameObject.FindGameObjectWithTag("Bank").GetComponent<RectTransform>().position);
		if (GameObject.Find("TextMoney") != null) moneyText = GameObject.Find("TextMoney").GetComponent<MoneyUIUpdate>();
		scale = SceneManager.GetActiveScene().name == "MainMenu" ? 0.07f : 0.14f ;
		gameObject.GetComponent<Animator>().speed = 5f * (Random.value - 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.localScale = new Vector3(Screen.height * scale,Screen.height * scale,1f);
		Vector2 force, coinPosition = (Vector2)Camera.main.ScreenToWorldPoint(gameObject.GetComponent<RectTransform>().position);
		force = coinPosition - coinDestination;
		force.Normalize();

		if (wait > 0f) wait -= Time.deltaTime;
		else gameObject.GetComponent<Rigidbody2D>().AddForce(force * -750f, ForceMode2D.Force);
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Bank") {
			if (moneyText != null) moneyText.addCoin();
			Destroy (gameObject);
		}
	}
}
