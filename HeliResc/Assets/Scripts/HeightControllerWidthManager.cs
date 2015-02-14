using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeightControllerWidthManager : MonoBehaviour {

	private RectTransform rect;
	public float widthPercentageByScreen = 0.1f;

	// Use this for initialization
	void Start () {
		rect = gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		rect.sizeDelta = new Vector2(Screen.width*widthPercentageByScreen,rect.sizeDelta.y);
	}
}
