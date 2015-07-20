using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelButtonHandler : MonoBehaviour {

	public Vector2 size;			//Size in percentage of the screen
    private string levelId;			//The name the level information is going to be saved
    private int id;					//The number of the level in the set

    public void Init(int id, LevelSet set) {
        this.id = id;
        levelId = set.levelSetName + id;			//Set the identifier from the set name and the integer id 
    }
    public void SetPosition(Vector3 position) {
        RectTransform rect = GetComponent<RectTransform>();

        transform.position = position;
		rect.sizeDelta = new Vector2 (size.x * Screen.width, size.y * Screen.height);
    }

	public void LoadLevel() {			
		GameManager.LoadLevel (levelId);
	}
}
