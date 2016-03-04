using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class flashOfWhiteManager : MonoBehaviour {

	public GameObject levelSet;
	public bool trigger = false, once = true;
	private int spam = 10;
	private LevelSetHandler handler;
	public GameObject coinPrefab;

	void Update () {
		if (trigger) {
			complete ();
			trigger = false;
		}
	}

	public bool setLevelSet (GameObject levelS) {
		levelSet = levelS;
		handler = levelSet.GetComponent<LevelSetHandler>();
		return levelSet != null ? true : false ;
	}

	public void playAnimation () {
		gameObject.GetComponent<Animator>().Play("flash");
		gameObject.GetComponent<Animator>().SetTrigger("end");
		gameObject.GetComponent<SoundObject>().PlaySound();

		GameObject.FindObjectOfType<GameManager>().wallet.AddMoney(handler.Set.completionReward);
		once = false;
	}

	public void complete () {
		handler.setChest.enabled = false;
		handler.setBG.sprite = handler.setBGCompleted;

		if (!once) {
			if (handler.Set.completionReward >= spam) StartCoroutine(CoinSpam(handler.Set.completionReward, handler.setChest.gameObject));
			else StartCoroutine(CoinFlow(handler.Set.completionReward, handler.setChest.gameObject));
			once = true;
		} 
	}

	private IEnumerator CoinFlow(int amount, GameObject spawnPosition) {

		// Coin initialization
		//yield return new WaitForSeconds(1f);
		GameObject[] coins = new GameObject[amount];
		//Debug.Log(coinDestination);
		for (int i = 0; i < amount; i++) {
			coins[i] = Instantiate(coinPrefab, spawnPosition.transform.position, Quaternion.identity) as GameObject;
			coins[i].transform.SetParent(GameObject.FindGameObjectWithTag("Bank").transform);
			Vector2 force = new Vector2 (Random.value-0.5f,Random.value-0.5f);
			force.Normalize();
			coins[i].GetComponent<Rigidbody2D>().AddForce(force * (Random.value * 15000f + 5000f), ForceMode2D.Force);
		}
		yield return null;
	}

	private IEnumerator CoinSpam(int amount, GameObject spawnPosition) {

		for (int i = 0; i < spam; i++){
			StartCoroutine(CoinFlow(amount/spam, spawnPosition));
			yield return new WaitForEndOfFrame();
		}
		yield return null;
	}
}
