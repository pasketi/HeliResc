using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CopterSelection : MonoBehaviour {

    private GameManager gameManager;

	public Sprite[] copterSprites;

	public Text engineText;
	public Text fuelText;
	public Text ropeText;
	public Image copterInfoImage;
	private GameObject copterInfoPanel;

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
            go.GetComponent<CopterEntryScript>().SetCopterInfo(i, copterSprites[i], this);
        }

		copterInfoPanel = GameObject.Find("PanelCopterInfo");
		copterInfoPanel.SetActive (false);
	}

	public void SetCopterInfoPanel(string engine, string fuel, string rope, int i) {
		copterInfoPanel.SetActive (true);
		copterInfoImage.sprite = copterSprites [i];
		engineText.text = engine;
		fuelText.text = fuel;
		ropeText.text = rope;
	}
}
