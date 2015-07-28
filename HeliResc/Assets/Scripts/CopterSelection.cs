using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CopterSelection : MonoBehaviour {

    private GameManager gameManager;							//Reference to the game manager

    private Dictionary<int, CopterEntryScript> copterEntries;	//The entries to show in the list of copters
    private Dictionary<int, CopterInfo> allCopters;				//Information about the copters

    public GridLayoutGroup group;								//Reference to layoutgroup component of the copterlist
    public GameObject copterEntry;								//Prefab of the copter entry to add to the list

    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		RectTransform t = transform as RectTransform;
		group.cellSize = new Vector2 (t.rect.width, t.rect.height * 0.25f);

		copterEntries = new Dictionary<int, CopterEntryScript> ();
		allCopters = gameManager.CopterInfos;

		for (int i = 0; i < gameManager.copters.Length; i++) {
			GameObject go = Instantiate(copterEntry) as GameObject;
			go.transform.SetParent(group.transform);
			CopterEntryScript script = go.GetComponent<CopterEntryScript>();
			script.SetInfo(i, this, allCopters[i]);
			copterEntries.Add(i, script);
		}
    } 
}
