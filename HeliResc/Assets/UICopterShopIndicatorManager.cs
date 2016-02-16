using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICopterShopIndicatorManager : MonoBehaviour {

	private GameManager gManager;
	private Image img;
	public float maxPulse = 1.2f, minPulse = 0.7f, step = 0.025f;
	private float Pulse = 1f;
	private bool up = false;

	// Use this for initialization
	void Start () {
		gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		img = gameObject.GetComponent<Image>();
		img.enabled = false;

		foreach (CopterInfo copter in gManager.CopterInfos.Values){
			if (copter.buyable &&
				copter.copterPrice <= gManager.wallet.Coins &&
				!copter.unlocked && 
				!img.isActiveAndEnabled)
				img.enabled = true;
			
			/*Debug.Log("Copter: " + copter.copterIndex + 
				" Buyable: " + copter.buyable + 
				" Unlocked: " + copter.unlocked + 
				" Price: " + copter.copterPrice + 
				" Stars: " + copter.requiredStars + 
				" Rubies: " + copter.requiredRubies +
				" Total: " + (copter.buyable &&
				copter.copterPrice <= gManager.wallet.Coins &&
				copter.requiredStars <= PlayerPrefs.GetInt(SaveStrings.sPlayerStars, 0) &&
				copter.requiredRubies <= PlayerPrefs.GetInt(SaveStrings.sPlayerRubies, 0) && 
				!copter.unlocked));*/
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (up) {
			Pulse += step;
			if (Pulse >= maxPulse) up = false;
		} else {
			Pulse -= step;
			if (Pulse <= minPulse) up = true;
		}

		img.transform.localScale = new Vector3(Pulse, Pulse, Pulse);
		//Funny things
		//img.transform.Rotate(new Vector3 (step * 360f, step * 360f, 0f));
	}
}
