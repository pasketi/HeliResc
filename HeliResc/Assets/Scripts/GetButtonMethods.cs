using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetButtonMethods : MonoBehaviour {


	public Button btnResetAll;
	public Button btnCoins;

	// Use this for initialization
	void Start () {
		GameManager g = GameObject.Find ("GameManager").GetComponent<GameManager>();

		
		btnResetAll.onClick.AddListener (() => g.resetData ());
		btnCoins.onClick.AddListener (() => g.wallet.AddMoney (5));
	}
}
