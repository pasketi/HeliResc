using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	private GameObject reset, saved, cargo;
	private RectTransform tempRect, cargoImage;
	private Text tempText;
	private float scale = 0.1f;
	private LevelManager manager;

	// Use this for initialization
	void Start () {
		manager = (LevelManager) GameObject.Find("LevelManagerO").GetComponent(typeof(LevelManager));
		reset = gameObject.transform.FindChild("Reset").gameObject;
		saved = gameObject.transform.FindChild("Saved").gameObject;
		cargo = gameObject.transform.FindChild("Cargo").gameObject;
		cargoImage = gameObject.transform.FindChild("cargoImage").gameObject.GetComponent<RectTransform>();

		tempText = reset.transform.FindChild("Text").gameObject.GetComponent<Text>();
		tempText.fontSize = (int)(Screen.height * scale);
		tempRect = reset.GetComponent<RectTransform>();
		tempRect.sizeDelta = new Vector2(tempText.fontSize*4f, tempText.fontSize + (tempText.fontSize/4f));

		tempText = saved.GetComponent<Text>();
		tempText.fontSize = (int)(Screen.height * scale);
		tempRect = saved.GetComponent<RectTransform>();
		tempRect.sizeDelta = new Vector2(tempText.fontSize*2f, (tempText.fontSize + (tempText.fontSize/4f)));
		tempRect.anchoredPosition = new Vector2((Screen.width*manager.uiLiftPowerWidth)+(tempText.fontSize*1f), -(tempText.fontSize + (tempText.fontSize/4f))/1.5f);

		tempText = cargo.GetComponent<Text>();
		tempText.fontSize = (int)(Screen.height * scale);
		tempRect = cargo.GetComponent<RectTransform>();
		tempRect.sizeDelta = new Vector2(tempText.fontSize*3f, (tempText.fontSize + (tempText.fontSize/4f)));
		tempRect.anchoredPosition = new Vector2((Screen.width*manager.uiLiftPowerWidth)+(tempText.fontSize*4.25f), -(tempText.fontSize + (tempText.fontSize/4f))/1.5f);
		cargoImage.sizeDelta = new Vector2((tempRect.sizeDelta.y*0.8f)*(51f/71f), tempRect.sizeDelta.y*0.8f);
		cargoImage.anchoredPosition = new Vector2((Screen.width*manager.uiLiftPowerWidth)+(tempText.fontSize*2.25f), (-(tempText.fontSize + (tempText.fontSize/4f))/2f)+((-(tempText.fontSize + (tempText.fontSize/4))/2)/4));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
