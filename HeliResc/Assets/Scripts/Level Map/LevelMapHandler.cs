using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelMapHandler : MonoBehaviour {

    public GameObject levelButtonPrefab;

    public RectTransform[] setPositions;

	// Use this for initialization
	void Start () {
        List<LevelSet> levels = LevelHandler.LevelSets;
        foreach (LevelSet ls in levels) {
            for (int i = 0; i < ls.levelAmount; i++) {
                GameObject go = Instantiate(levelButtonPrefab) as GameObject;
                go.GetComponent<LevelButtonHandler>().Init(i, ls);
            }
        }
	}

    private Vector2 CalculateButtonPosition(int index) {
        Vector2 vec = new Vector2();
        
        return vec;
    }
}
