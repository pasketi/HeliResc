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
	
	public GameObject loadImage;

	public Vector2 size;			//Size in percentage of the screen
    private string levelId;			//The name the level information is going to be saved
    private Level level;            //Contains the information about the level the button will open

    public void Init(int id, LevelSet set) {
        levelId = set.levelSetName + id;			//Set the identifier from the set name and the integer id
        level = Level.Load(set.levelSetName, id);
		loadImage = GameObject.Find("LoadImage");

        transform.localScale = Vector3.one;
				Debug.Log(loadImage + " " + GameObject.Find("LoadImage"));

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


        rect.anchoredPosition = position;
        

        Vector2 vec = rect.root.GetComponent<RectTransform>().sizeDelta;

        //rect.sizeDelta = new Vector2(Screen.width * size.x, Screen.height * size.y);
    }

    public IEnumerator SetPositionAnimated(Vector3 end) {
        RectTransform rect = GetComponent<RectTransform>();
        //rect.sizeDelta = new Vector2(Screen.width * size.x, Screen.height * size.y);
        
        Vector2 dir = end.normalized;               //Assign the direction the button will move to
        rect.anchoredPosition = Vector2.zero;       //Make sure the button starts from the middle of the set
        Vector2 start = rect.anchoredPosition;      //Note the start position so in the while loop it can be compared
        float dist = Vector2.Distance(start, end);  //The distance from the middle of set to the buttons final position. The amount is in pixels


        while(Vector2.Distance(rect.anchoredPosition, start) <= dist) {
            rect.anchoredPosition += (dir * dist * Time.deltaTime);
            yield return null;
        }

        rect.anchoredPosition = end;
        
    }

	public void LoadLevel() {
        if (level.unlocked == true) {
            LevelHandler.CurrentLevel = level;
			if (level.id > 0){
				loadImage.SetActive(true);
                GameManager.LoadLevel(levelId);
			}
			else{
				loadImage.SetActive(true);
                GameManager.LoadLevel("IntroScreen");
			}
        }
	}
}
