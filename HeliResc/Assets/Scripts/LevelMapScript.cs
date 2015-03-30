using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelMapScript : MonoBehaviour {

	private RectTransform t;	//Reference to the button that will be centered on screen.

	private RectTransform rect;	//Reference to the panel's rect transform

	// Use this for initialization
	void Awake () {
		rect = GetComponent<RectTransform>();
		rect.sizeDelta = new Vector2(Screen.width, Screen.height) * 2f;

		int currentLevel = 1;

		if(PlayerPrefs.HasKey("CurrentLevel"))
		   currentLevel = PlayerPrefs.GetInt("CurrentLevel");
		else
			PlayerPrefs.SetInt("CurrentLevel", 1);

		t = transform.Find("LevelButton" + currentLevel.ToString()).GetComponent<RectTransform>();

		SetScrollRectPosition(t);
	}

	/// <summary>
	/// Sets the panels position so that the target is in the middle of the screen
	/// </summary>
	/// <param name="target">Target.</param>
	private void SetScrollRectPosition(RectTransform target) {
		Vector3 middlePoint = new Vector3(Screen.width, Screen.height) * 0.5f;
		
		Vector3 newPos = (middlePoint - target.localPosition);

		newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
		newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

		rect.anchoredPosition = newPos;
	}

}
