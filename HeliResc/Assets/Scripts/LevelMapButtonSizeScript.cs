using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelMapButtonSizeScript : MonoBehaviour {

	public Vector2 v;

	// Use this for initialization
	void Start () {
		RectTransform r = GetComponent<RectTransform>();
		r.anchorMin -= v * .5f;
		r.anchorMax = r.anchorMin + v;
	}
}