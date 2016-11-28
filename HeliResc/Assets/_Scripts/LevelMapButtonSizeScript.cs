using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelMapButtonSizeScript : MonoBehaviour {

	public Vector2 v;

	// Use this for initialization
	void Awake () {
		RectTransform r = GetComponent<RectTransform>();

		float size = GameObject.FindObjectOfType<LevelMapScript> ().size;

		//v /= size;

        Vector2 vec = r.root.GetComponent<RectTransform>().sizeDelta;

        r.sizeDelta = new Vector2(vec.x * v.x, vec.y * v.y);

		//r.anchorMin -= v * (.5f);
		//r.anchorMax = r.anchorMin + v;
	}
}