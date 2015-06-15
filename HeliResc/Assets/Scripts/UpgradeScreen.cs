using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UpgradeScreen : MonoBehaviour {

    public GameObject upgradeEntry;             //The upgrade button to insert into the panel    

    private RectTransform rTransform;           //the panels rect transform
    private GridLayoutGroup grid;               //Reference to grid layout group
    private Vector2 panelSize;                  //Size of the panel in pixels

	// Use this for initialization
	void Start () {
        grid = GetComponent<GridLayoutGroup>();
        rTransform = GetComponent<RectTransform>();        

        List<Upgrade> ups = new List<Upgrade>();
        Copter c = null;// GameObject.Find("GameManager").GetComponent<GameManager>().CurrentCopterScript;
        return;
        foreach (Upgradable u in c.Upgrades.Values) {
            if(u.allowUpgrade == true)
                ups.Add(u.upgrade);
        }

        List<GameObject> entries = new List<GameObject>();
        foreach (Upgrade u in ups) {
            GameObject entry = Instantiate(upgradeEntry) as GameObject;

            entry.GetComponent<UpgradeButton>().Upgrade = u;

            entries.Add(entry);
            entry.transform.parent = rTransform;            
        }

        CalculatePanelSize(ups.Count);
	}

    private void CalculatePanelSize(int amount) {
        //Calculate the panels size
        float x = (rTransform.anchorMax.x - rTransform.anchorMin.x) * Screen.width;
        float y = (rTransform.anchorMax.y - rTransform.anchorMin.y) * Screen.height;
        panelSize = new Vector2(x, y);
        grid.cellSize = new Vector2(x / amount, y);
    }
	
}
