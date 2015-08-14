using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelMapScript : MonoBehaviour {

	public float size = 2f;
	public string set;

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
		SetScrollRectPosition ();
	}

	private void SetScrollRectPosition() {
		RectTransform rect = GetComponent<RectTransform> ();
		LevelSetHandler[] sets = GameObject.FindObjectsOfType<LevelSetHandler> ();

		RectTransform target = null;

		foreach (LevelSetHandler s in sets) {
			if(s.setName.Equals(LevelHandler.CurrentLevel.setName)) {
				target = s.GetComponent<RectTransform>();
				//Debug.Log ("Target: " + s.gameObject.name);
				//Debug.Log ("Position: " + s.gameObject.transform.localPosition);
			}
		}

		Vector3 range = new Vector3(Screen.width, Screen.height) * 0.5f;

		Vector3 newPos = (-target.localPosition);
		//Debug.Log ("New pos: " + newPos);

		newPos.x = Mathf.Clamp(newPos.x, -range.x, range.x);
		newPos.y = Mathf.Clamp(newPos.y, -range.y, range.y);

		//Debug.Log ("New pos: " + newPos);

		rect.anchoredPosition = newPos;
	}

    private void BackButton() {
        GameObject.Find("GameManager").GetComponent<GameManager>().loadMainMenu(false, null, 0);
    }
}
