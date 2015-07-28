using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelButtonHandler : MonoBehaviour {
    
    //Sprites to assign to stars and ruby if they are unlocked
    public Sprite unlockedStar;
    public Sprite unlockedRubySprite;

    public Image lockedImage;       //The image component of the locked sprite. enable if level is locked
    public Image[] unlockedStars;   //The image components of the stars.
    public Image unlockedRuby;      //The image component of the ruby image

	public Vector2 size;			//Size in percentage of the screen
    private string levelId;			//The name the level information is going to be saved
    private int id;					//The number of the level in the set
    private Level level;            //Contains the information about the level the button will open

    public void Init(int id, LevelSet set) {
        this.id = id;
        levelId = set.levelSetName + id;			//Set the identifier from the set name and the integer id
        level = Level.Load(set.levelSetName, id);

        //Debug.Log(level.ToString());

        if(level.unlocked == true) {
            if (unlockedStars.Length != 3) Debug.LogError("The stars array is not the correct size");

            if (level.star1 == true) unlockedStars[0].sprite = unlockedStar;
            if (level.star2 == true) unlockedStars[1].sprite = unlockedStar;
            if (level.star3 == true) unlockedStars[2].sprite = unlockedStar;
            if (level.rubyFound == true) unlockedRuby.sprite = unlockedRubySprite;
            lockedImage.enabled = false;

        } else {
            lockedImage.enabled = true;
            foreach(Image i in unlockedStars)
                i.enabled = false;
            unlockedRuby.enabled = false;
        }

    }
    public void SetPosition(Vector3 position) {
        RectTransform rect = GetComponent<RectTransform>();

        transform.position = position;
		rect.sizeDelta = new Vector2 (size.x * Screen.width, size.y * Screen.height);
    }

	public void LoadLevel() {
        //if (level.unlocked == true) {
            LevelHandler.UpdateCurrentLevel(level);
            GameManager.LoadLevel(levelId);
        //}
	}
}
