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
        star1.sprite = levelEnd.star1 ? unlockedStar : lockedStar;
        star2.sprite = levelEnd.star2 ? unlockedStar : lockedStar;
        star3.sprite = levelEnd.star3 ? unlockedStar : lockedStar;

        text.text = "You got " + levelEnd.itemsSaved.ToString() + " crates and earned " + levelEnd.collectedCoins.ToString() + " coins";
    }
}
