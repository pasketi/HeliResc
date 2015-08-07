using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelMapScript : MonoBehaviour {

	public float size = 2f;


    void OnEnable() {
        EventManager.StartListening(SaveStrings.eEscape, BackButton);
    }
    void OnDisable() {
        EventManager.StopListening(SaveStrings.eEscape, BackButton);
    }

	// Use this for initialization
	void Awake () {

		RectTransform rect = GetComponent<RectTransform>();
		rect.sizeDelta = new Vector2(Screen.width, Screen.height) * size;
	}

	/// <summary>
	/// Sets the panels position so that the target is in the middle of the screen
	/// </summary>
	/// <param name="target">Target.</param>
//	private void SetScrollRectPosition(RectTransform target) {
//		Vector3 middlePoint = new Vector3(Screen.width, Screen.height) * 0.5f;
//		
//		Vector3 newPos = (middlePoint - target.localPosition);
//
//		newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
//		newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);
//
//		rect.anchoredPosition = newPos;
//	}

    private void BackButton() {
        GameObject.Find("GameManager").GetComponent<GameManager>().loadMainMenu(false, null, 0);
    }
}
