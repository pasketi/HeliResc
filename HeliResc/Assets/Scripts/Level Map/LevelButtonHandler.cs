using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelButtonHandler : MonoBehaviour {

    private string levelId;
    private int id;

    public void Init(int id, LevelSet set) {
        this.id = id;
        levelId = set.levelSetName + id;
    }
    public void SetPosition(Vector3 position) {
        RectTransform rect = GetComponent<RectTransform>();

        transform.position = position;

        //rect.anchorMin = position;
        //rect.anchorMax = position;

        
    }
}
