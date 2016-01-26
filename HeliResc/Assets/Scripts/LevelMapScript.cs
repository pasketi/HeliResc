using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelMapScript : MonoBehaviour {

	public float size = 2f;
	public string set;

    public ScrollRect scrollRect;

    private Vector2 minLimit;
    private Vector2 maxLimit;
    private Vector3 range;
    private RectTransform rect;

    void OnEnable() {
        EventManager.StartListening(SaveStrings.eEscape, BackButton);
    }
    void OnDisable() {
        EventManager.StopListening(SaveStrings.eEscape, BackButton);
    }

	// Use this for initialization
	void Awake () {

		rect = GetComponent<RectTransform>();
        range = rect.root.GetComponent<RectTransform>().sizeDelta;

        minLimit = -(new Vector2(range.x * ((size * 0.5f) - 1), range.y * ((size * 0.5f) - 1)));
        maxLimit = new Vector2(range.x * (size * 0.5f), range.y * (size * 0.5f));

        //rect.sizeDelta = range * size;

		SetScrollRectPosition ();
	}

    public IEnumerator MoveToNextSet(RectTransform start, RectTransform end, float time) {

        scrollRect.enabled = false;

        Vector2 dir = (end.anchoredPosition - start.anchoredPosition).normalized;
        Vector2 startingPoint = (-start.localPosition) + (range * 0.5f);

        rect.anchoredPosition = startingPoint;
        LimitScrollRectPosition();

        float dist = Vector2.Distance(start.anchoredPosition, end.anchoredPosition);
        float travelled = 0;
        float step = dist / time;        

        while (travelled < dist) {
            travelled += step * Time.deltaTime;

            Vector2 newPos = rect.anchoredPosition - (dir * step * Time.deltaTime);
            rect.anchoredPosition = newPos;

            LimitScrollRectPosition();
            
            yield return null;
        }

        Vector2 vec = startingPoint - (dir * dist);        
        rect.anchoredPosition = vec;
        LimitScrollRectPosition();
        scrollRect.enabled = true;
    }

	private void SetScrollRectPosition() {
		LevelSetHandler[] sets = GameObject.FindObjectsOfType<LevelSetHandler> ();

		RectTransform target = null;

		foreach (LevelSetHandler s in sets) {
			if(s.setName.Equals(LevelHandler.CurrentLevel.setName)) {
				target = s.GetComponent<RectTransform>();
				//Debug.Log ("Target: " + s.gameObject.name);
				//Debug.Log ("Position: " + s.gameObject.transform.localPosition);
			}
		}

        Vector3 newPos = (-target.localPosition) + (range * 0.5f);
        rect.anchoredPosition = newPos;

        LimitScrollRectPosition();
	}

    private void LimitScrollRectPosition() { 
        Vector3 vec = rect.anchoredPosition;
        vec.x = Mathf.Clamp(vec.x, minLimit.x, maxLimit.x);
        vec.y = Mathf.Clamp(vec.y, minLimit.y, maxLimit.y);
        rect.anchoredPosition = vec;
    }

    private void BackButton() {
        GameObject.Find("GameManager").GetComponent<GameManager>().loadMainMenu(false, null, 0);
    }
}
