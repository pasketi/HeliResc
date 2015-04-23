using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetButtonMethods : MonoBehaviour {


	public Button btnUpFuel;
	public Button btnUpEngine;
	public Button btnUpRope;
	public Button btnResetAll;
	public Button btnResetUps;
	public Button btnChangeCopter;
	public Button btnPlay;

	private GameManager g;
	// Use this for initialization
	void Start () {
		g = GameObject.Find ("GameManager").GetComponent<GameManager>();

		btnUpFuel.onClick.AddListener(() => g.upgradeCurrentFuelTank());
		btnUpEngine.onClick.AddListener(() => g.upgradeCurrentEngine());
		btnUpRope.onClick.AddListener(() => g.upgradeCurrentRope());

		//btnResetUps.onClick.AddListener(() => g.resetUpgrades());
		//btnResetAll.onClick.AddListener (() => g.resetData ());

		btnChangeCopter.onClick.AddListener(() => g.swapCopter());
	
		btnPlay.onClick.AddListener (() => g.startGame("LevelMap"));
	}
}
