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

	void Start() {
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		if (!gameManager.ShowLevelEnd)
			return;
		levelEnd = gameManager.levelEnd;
		star1.sprite = levelEnd.star1 ? unlockedStar : lockedStar;
		star2.sprite = levelEnd.star2 ? unlockedStar : lockedStar;
		star3.sprite = levelEnd.star3 ? unlockedStar : lockedStar;

		text.text = "You got " + levelEnd.itemsSaved.ToString() + " crates";
	}

	void Update () {
	
	}
}
