using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEndManager : MonoBehaviour {

	private GameManager gameManager;

	public Text text;
	public Image star1, star2, star3;

    public Image alsFace;

	public Sprite unlockedStar;
	public Sprite lockedStar;

	private LevelEndInfo levelEnd;

    private LevelInfo level;
    private LevelInfo next;

    private string message;

    private int starsEarned;


    public void UpdateLevelEnd(GameManager gm) {
        gameManager = gm;

        levelEnd = gameManager.levelEnd;

        level = SaveLoad.LoadLevelInfo(levelEnd.index);
        next = SaveLoad.LoadLevelInfo(levelEnd.index + 1);

        starsEarned = 0;

        if (levelEnd.star1)
            starsEarned++;
        if (levelEnd.star2)
            starsEarned++;
        if (levelEnd.star3)
            starsEarned++;


        if (levelEnd.passedLevel) {
            PassedLevel();
        }
        else {
            FailedLevel();
        }        

        level.star1 = level.star1 || levelEnd.star1;
        level.star2 = level.star2 || levelEnd.star2;
        level.star3 = level.star3 || levelEnd.star3;

        SaveLoad.SaveLevelInfo(level);

        text.text = message;
    }

    private void FailedLevel() {
        star1.sprite = lockedStar;
        star2.sprite = lockedStar;
        star3.sprite = lockedStar;

        alsFace.transform.Rotate(new Vector3(0,0, 180));

        message = "You have failed this level";
    }
    private void PassedLevel() {

        message = "You win! Awesome job!";

        star1.sprite = starsEarned > 0 ? unlockedStar : lockedStar;
        star2.sprite = starsEarned > 1 ? unlockedStar : lockedStar;
        star3.sprite = starsEarned > 2 ? unlockedStar : lockedStar;

        if (next.locked)
        {
            PlayerPrefs.SetInt("CurrentLevel", next.index);
            next.locked = false;
            next.SetStars();
            SaveLoad.SaveLevelInfo(next);
        }
    }

    public void PressRestart() {
        Application.LoadLevel("Level" + levelEnd.index);
    }

    public void PressLevelMap() {
        Application.LoadLevel("LevelMap");
    }

    public void PressNextLevel() {
        Application.LoadLevel("Level" + (levelEnd.index + 1));
    }
}
