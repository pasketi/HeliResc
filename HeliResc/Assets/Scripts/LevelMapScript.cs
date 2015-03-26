using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelMapScript : MonoBehaviour {

	public RectTransform t;	//Need the get this via script later

	private RectTransform rect;

	// Use this for initialization
	void Awake () {
		rect = GetComponent<RectTransform>();
		rect.sizeDelta = new Vector2(Screen.width, Screen.height) * 2f;

		SetScrollRectPosition(t);
	}

	/// <summary>
	/// Sets the panels position so that the target is in the middle of the screen
	/// </summary>
	/// <param name="target">Target.</param>
	private void SetScrollRectPosition(RectTransform target) {
		Vector3 middlePoint = new Vector3(Screen.width, Screen.height) * 0.5f;
		
		rect.anchoredPosition = (middlePoint - target.localPosition);
	}

}
