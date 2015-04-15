using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEndManager : MonoBehaviour {

	private GameManager gameManager;

	public Text text;
	public Image star1, star2, star3;

	public Sprite unlockedStar;
	public Sprite lockedStar;

	private LevelEndInfo levelEnd;


    public void UpdateLevelEnd(GameManager gm) {
        gameManager = gm;

        levelEnd = gameManager.levelEnd;
        LevelInfo level = SaveLoad.LoadLevelInfo(levelEnd.index);
        LevelInfo next = SaveLoad.LoadLevelInfo(levelEnd.index + 1);

        star1.sprite = levelEnd.star1 || level.star1 ? unlockedStar : lockedStar;
        star2.sprite = levelEnd.star2 || level.star2 ? unlockedStar : lockedStar;
        star3.sprite = levelEnd.star3 || level.star3 ? unlockedStar : lockedStar;

        if(levelEnd.passedLevel && next.locked) {
            next.locked = false;
            next.SetStars();
            SaveLoad.SaveLevelInfo(next);
        }

        level.star1 = level.star1 || levelEnd.star1;
        level.star2 = level.star2 || levelEnd.star2;
        level.star3 = level.star3 || levelEnd.star3;

        SaveLoad.SaveLevelInfo(level);

        text.text = "You got " + levelEnd.itemsSaved.ToString() + " crates and earned " + levelEnd.collectedCoins.ToString() + " coins";
    }
}
