using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSetHandler : MonoBehaviour {

    public GameObject lockedPanel;
    public GameObject buttonPrefab; //Prefab of a button to open levels    
    public Image setImage;          //The image UI-component in the middle of the button set
    public string setName;
    
    private LevelSet set;            //The kind of set the group has

    void Start() {

        set = LevelHandler.GetLevelSet(setName);

        int playerStars = PlayerPrefs.GetInt(SaveStrings.sPlayerStars);

        if (playerStars >= set.neededStars && set.unlocked) {
            SetUnlocked();
        }
        else {
            SetLocked(playerStars);
        }
    }

    private void SetLocked(int playerStars) {
        lockedPanel.SetActive(true);
        setImage.enabled = false;

        Text starText = lockedPanel.GetComponentInChildren<Text>();
        starText.text = playerStars + "/" + set.neededStars;
        Debug.Log("Text: " + (starText == null));
    }

    private void SetUnlocked() {

        setImage.enabled = true;
        lockedPanel.SetActive(false);
        
		float mapSize = GameObject.FindObjectOfType<LevelMapScript> ().size;
		for (int i = 0; i < set.levelAmount; i++) {
            GameObject go = Instantiate(buttonPrefab) as GameObject;
            go.transform.SetParent(transform);
            LevelButtonHandler handler = go.GetComponent<LevelButtonHandler>();
            handler.Init(i, set);

            handler.SetPosition(CalculateButtonPosition(i, mapSize));
        }
        if (set.setImage == null)
            setImage.enabled = false;
        else
            setImage.sprite = set.setImage;
    }

    private Vector3 CalculateButtonPosition(int index, float mapSize) {
        Vector3 vec = new Vector2();
        float angle = (360f / set.levelAmount) * index * Mathf.Deg2Rad;

        float x = Mathf.Sin(angle);
        float y = Mathf.Cos(angle);       

        vec.x = x;
        vec.y = y;
		float height = 0;
		RectTransform rect = GetComponent<RectTransform>();

		//Depending on the aspect ratio set the circle's radius from the rectangle's width or height
		float h = ((rect.anchorMax.y - rect.anchorMin.y) * Screen.height);
		float w = ((rect.anchorMax.x - rect.anchorMin.x) * Screen.width);
		if (h <= w) {
			height = (h * 0.5f * mapSize) * .99451f;
		} else {
			height = (w * 0.5f * mapSize) * .99451f;
		}


		vec *= (height);		 

        return transform.position + vec;
    }
}

[System.Serializable]
public class LevelSet {   

	public string levelSetName;     //The identifier of the set. Crate, swimmer etc.
	public int levelAmount;         //how many levels is in the set
	public Sprite setImage;         //The image to show in the middle of the set
	public int neededStars;			//How many stars is required to open the set
    public int setIndex;            //the index of the set in the list of level sets
    public bool unlocked;           //Is the set unlocked

    public string challenge1;
    public string challenge2;
    public string challenge3;

    public void Save() {
        PlayerPrefsExt.SetBool(levelSetName + "Set", unlocked);
    }
    public void Load() {
        unlocked = PlayerPrefsExt.GetBool(levelSetName + "Set") || unlocked;
    }
}