using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CopterSelection : MonoBehaviour {

    private GameManager gameManager;

    public GridLayoutGroup group;
    public GameObject copterEntry;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindObjectOfType<GameManager>();

        RectTransform t = transform as RectTransform;       
        group.cellSize = new Vector2(t.rect.width - 10, group.cellSize.y);

        for(int i = 0; i < gameManager.CopterAmount; i++) {
            GameObject go = Instantiate(copterEntry) as GameObject;
            go.transform.SetParent(group.transform);
            go.GetComponent<CopterEntryScript>().SetCopterInfo(i);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
