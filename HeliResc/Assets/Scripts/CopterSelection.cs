using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CopterSelection : MonoBehaviour {

    private GameManager gameManager;

    private Dictionary<int, CopterEntryScript> copterEntries;
    private Dictionary<string, Copter> allCopters;

    public Button b0;
    public Button b1;
    public Button b2;

	public Text engineText;
	public Text fuelText;
    public Text ropeText;
	

    public GridLayoutGroup group;
    public GameObject copterEntry;

    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        b0.onClick.AddListener(() => SetCopter(0));
        b1.onClick.AddListener(() => SetCopter(1));
        b2.onClick.AddListener(() => SetCopter(2));
    }

    void SetCopter(int i) {
        gameManager.setCurrentCopter(i);
    }

	// Use this for initialization
    //void Start () {
    //    gameManager = GameObject.FindObjectOfType<GameManager>();

    //    RectTransform t = transform as RectTransform;       
    //    group.cellSize = new Vector2(t.rect.width - 10, group.cellSize.y);

    //    copterEntries = new Dictionary<int, CopterEntryScript>();

    //    for(int i = 0; i < gameManager.CopterAmount; i++) {
    //        GameObject go = Instantiate(copterEntry) as GameObject;
    //        go.transform.SetParent(group.transform);
    //        CopterEntryScript script = go.GetComponent<CopterEntryScript>();
    //        script.SetCopterInfo(i, copterSprites[i], this);

    //        copterEntries.Add(i, script);
    //    }

    //    copterInfoPanel = GameObject.Find("PanelCopterInfo");
    //    copterInfoPanel.SetActive (false);
    //}

    //public void SetCopterInfoPanel(string engine, string fuel, string rope, int i) {
    //    copterInfoPanel.SetActive (true);
    //    copterInfoImage.sprite = copterSprites [i];
    //    engineText.text = engine;
    //    fuelText.text = fuel;
    //    ropeText.text = rope;
    //}

    ///// <summary>
    ///// Set the buy/select button
    ///// </summary>
    ///// <param name="index"></param>
    //public void UpdateEntry(int index) {
    //    copterEntries[index].SetCopterInfo(index, copterSprites[index], this);
    //}
}
