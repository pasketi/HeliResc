using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelMapButtonSizeScript : MonoBehaviour {

	RectTransform r;

	public Vector2 v;

	// Use this for initialization
	void Start () {
		r = GetComponent<RectTransform>();
		r.anchorMin -= v * .5f;
		r.anchorMax = r.anchorMin + v;
	}
}
