using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSetHandler : MonoBehaviour {

    public GameObject buttonPrefab; //Prefab of a button to open levels
    public LevelSet set;            //The kind of set the group has
    public float circleRadius;      //The radius the buttons are going to be from the middle in percentage of the screen height
    public Image setImage;          //The image to show in the middle of the button set

    void Start() {
        for (int i = 0; i < set.levelAmount; i++) {
            GameObject go = Instantiate(buttonPrefab) as GameObject;
            go.transform.SetParent(transform);
            LevelButtonHandler handler = go.GetComponent<LevelButtonHandler>();
            handler.Init(i, set);

            handler.SetPosition(CalculateButtonPosition(i));
        }
        if (set.setImage == null)
            setImage.enabled = false;
        else
            setImage.sprite = set.setImage;
    }

    private Vector3 CalculateButtonPosition(int index) {
        Vector3 vec = new Vector2();
        float angle = (360f / set.levelAmount) * index * Mathf.Deg2Rad;

        float x = Mathf.Sin(angle);
        float y = Mathf.Cos(angle);       

        vec.x = x;
        vec.y = y;

		vec *= (circleRadius * Screen.height);

        RectTransform rect = GetComponent<RectTransform>();

        return transform.position + vec;
    }
}

[System.Serializable]
public class LevelSet {
	public string levelSetName;     //The identifier of the set. Crate, swimmer etc.
	public int levelAmount;         //how many levels is in the set
	public Sprite setImage;         //The image to show in the middle of the set
	public int neededStars;			//How many stars is required to open the set
}