using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour {

    public RectTransform ContentParent;
    public GameObject prefab;
    public Sprite star, rubySprite;
    public Text TotalTimeText;
    private GameObject temp;
    private float TotalTime;

	// Use this for initialization
	void Start () {
        foreach (Level level in LevelHandler.GetLevels())
        {
            if (level.unlocked && level.star1)
            {
                temp = Instantiate(prefab) as GameObject;
                Text LevelNumber = temp.transform.FindChild("LevelNumber").GetComponent<Text>();
                Text BestTime = temp.transform.FindChild("Time").GetComponent<Text>();

                string LevelSetNumber;

                switch (level.setName)
                {
                    case "Tutorial0":
                        LevelSetNumber = "0-1";
                        LevelNumber.text = LevelSetNumber;
                        break;
                    case "Tutorial1":
                        LevelSetNumber = "0-2";
                        LevelNumber.text = LevelSetNumber;
                        break;
                    case "Tutorial2":
                        LevelSetNumber = "0-3";
                        LevelNumber.text = LevelSetNumber;
                        break;
                    default:
                        LevelSetNumber = (LevelHandler.GetLevelSet(level.setName).setIndex - 2).ToString();
                        LevelNumber.text = LevelSetNumber + "-" + (level.id + 1).ToString();
                        break;
                }

                Image star1 = temp.transform.FindChild("Star1").GetComponent<Image>();
                if (level.star1)
                    star1.sprite = star;
                Image star2 = temp.transform.FindChild("Star2").GetComponent<Image>();
                if (level.star2)
                    star2.sprite = star;
                Image star3 = temp.transform.FindChild("Star3").GetComponent<Image>();
                if (level.star3)
                    star3.sprite = star;
                Image ruby = temp.transform.FindChild("Ruby").GetComponent<Image>();
                if (level.rubyFound)
                    ruby.sprite = rubySprite;

                if (!level.star3)
                {
                    //level.bestTime = 9999f;
                    //BestTime.text = (level.bestTime / 60f >= 1f ? Mathf.Floor(level.bestTime / 60f).ToString("##:") : "") + (level.bestTime % 60f).ToString("00.00");
                }
                else
                {
                    TotalTime += level.bestTime;
                    BestTime.text = (level.bestTime / 60f >= 1f ? Mathf.Floor(level.bestTime / 60f).ToString("##:") : "") + (level.bestTime % 60f).ToString("00.00");
                }

                ContentParent.sizeDelta = new Vector2(ContentParent.sizeDelta.x, ContentParent.sizeDelta.y + 50);
                temp.transform.SetParent(ContentParent);
                temp = null;
            }
        }

        TotalTimeText.text = (TotalTime / 60f >= 1f ? Mathf.Floor(TotalTime / 60f).ToString("##:") : "") + (TotalTime % 60f).ToString("00.00"); ;
    }
}
