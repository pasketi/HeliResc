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

    public IEnumerator MoveToNextSet(RectTransform start, RectTransform end, float time) {
        
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 startingPoint = rect.anchoredPosition;
        Vector2 dir = (end.anchoredPosition - start.anchoredPosition).normalized;
        Vector2 range = new Vector2(Screen.width, Screen.height);
        float dist = Vector2.Distance(start.anchoredPosition, end.anchoredPosition);
        float travelled = 0;
        float step = dist / time;        

        while (travelled < dist) {
            travelled += step * Time.deltaTime;

            Vector2 newPos = rect.anchoredPosition - (dir * step * Time.deltaTime);
            newPos.x = Mathf.Clamp(newPos.x, 0, range.x);
		    newPos.y = Mathf.Clamp(newPos.y, 0, range.y);

            rect.anchoredPosition = newPos;
            yield return null;
        }

        Vector2 vec = startingPoint - (dir * dist);
        vec.x = Mathf.Clamp(vec.x, 0, range.x);
        vec.y = Mathf.Clamp(vec.y, 0, range.y);
        rect.anchoredPosition = vec;
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

		Vector3 range = new Vector3(Screen.width, Screen.height);

		Vector3 newPos = (-target.localPosition) + (range * 0.5f);
		//Debug.Log ("New pos: " + newPos);

		newPos.x = Mathf.Clamp(newPos.x, 0, range.x);
		newPos.y = Mathf.Clamp(newPos.y, 0, range.y);

		//Debug.Log ("New pos: " + newPos);

		rect.anchoredPosition = newPos;
	}

    private void BackButton() {
        GameObject.Find("GameManager").GetComponent<GameManager>().loadMainMenu(false, null, 0);
    }
}
