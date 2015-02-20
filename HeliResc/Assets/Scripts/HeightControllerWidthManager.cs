using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeightControllerWidthManager : MonoBehaviour {

	private RectTransform rect;
	private LevelManager manager;

	// Use this for initialization
	void Start () {
		manager = (LevelManager) GameObject.Find("LevelManagerO").GetComponent(typeof(LevelManager));
		rect = gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		rect.sizeDelta = new Vector2(Screen.width*manager.uiLiftPowerWidth,rect.sizeDelta.y);
	}
}
