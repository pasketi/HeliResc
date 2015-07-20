using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSetHandler : MonoBehaviour {

    public GameObject buttonPrefab; //Prefab of a button to open levels
    public LevelSet set;            //The kind of set the group has
    public float circleRadius;      //The radius the buttons are going to be from the middle

    void Start() {
        for (int i = 0; i < set.levelAmount; i++) {
            GameObject go = Instantiate(buttonPrefab) as GameObject;
            go.transform.SetParent(transform);
            LevelButtonHandler handler = go.GetComponent<LevelButtonHandler>();
            handler.Init(i, set);

            handler.SetPosition(CalculateButtonPosition(i));
        }
    }

    private Vector3 CalculateButtonPosition(int index) {
        Vector3 vec = new Vector2();
        float angle = (360f / set.levelAmount) * index * Mathf.Deg2Rad;

        float x = Mathf.Sin(angle);
        float y = Mathf.Cos(angle);       

        vec.x = x;
        vec.y = y;

        vec *= circleRadius;        

        RectTransform rect = GetComponent<RectTransform>();

        return transform.position + vec;
    }
}
