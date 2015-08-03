using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelEndManager : MonoBehaviour {

	private GameManager gameManager;

	public Image star1, star2, star3;
    public Image rubyImage;

    public Image alsFace;

    public Sprite drownedAl;
    public Sprite explodedAl;
    public Sprite winnerAl;
    public Sprite passedAl;
    public Sprite timeoutAl;
	public Sprite loserAl;

    private Dictionary<int, Sprite> endFaces;

	public Sprite unlockedStar;
	public Sprite lockedStar;
    public Sprite unlockedRuby;

	private LevelEndInfo levelEnd;
    private Level level;

    private Animator animator;

    private int starsEarned;


    public void UpdateLevelEnd(GameManager gm) {
        gameManager = gm;

        CreateFaceDictionary();

        levelEnd = gameManager.levelEnd;
		if (levelEnd == null) {
			Debug.LogError("Level end is null");
		}
        level = levelEnd.level;

		if (level == null) {
			Debug.LogError("Level end is null");
		}

        starsEarned = 0;

        alsFace.sprite = endFaces[levelEnd.endCondition];

        if (levelEnd.obj1Passed) starsEarned++;
        if (levelEnd.obj2Passed) starsEarned++;
        if (levelEnd.obj3Passed) starsEarned++;

		//Save the amount of stars and rubies the player has earned from all levels
		int playerStars = PlayerPrefs.GetInt(SaveStrings.sPlayerStars, 0);

		if (level.star1 == false && levelEnd.obj1Passed == true)
			playerStars++;
		if (level.star2 == false && levelEnd.obj2Passed == true)
			playerStars++;
		if (level.star3 == false && levelEnd.obj3Passed == true)
			playerStars++;

		PlayerPrefs.SetInt (SaveStrings.sPlayerStars, playerStars);

		if (level.rubyFound == false && levelEnd.rubyFound == true) {
			int rubies = PlayerPrefs.GetInt(SaveStrings.sPlayerRubies) + 1;
			PlayerPrefs.SetInt(SaveStrings.sPlayerRubies, rubies);
		}

		level.rubyFound = levelEnd.rubyFound || level.rubyFound;
        level.star1 = levelEnd.obj1Passed || level.star1;
        level.star2 = levelEnd.obj2Passed || level.star2;
        level.star3 = levelEnd.obj3Passed || level.star3;

        if (levelEnd.rubyFound == true)
			rubyImage.sprite = unlockedRuby;
		else
			rubyImage.enabled = false;
        if (levelEnd.passedLevel == true) {
            PassedLevel();
        }
        else {
            FailedLevel();
        }

        StartCoroutine(Animations(levelEnd.rubyFound));
		Level.Save (level);

    }

    private IEnumerator Animations(bool showRuby) {
        yield return null;
    }
    private IEnumerator CoinFlow(int amount) {
        yield return null;
    }
    private IEnumerator StarsAnimation() {
        yield return null;
    }
    private IEnumerator RubyFound() {
        yield return null;
    }    

    private void FailedLevel() {
        star1.sprite = lockedStar;
        star2.sprite = lockedStar;
        star3.sprite = lockedStar;

    }
    private void PassedLevel() {
        star1.sprite = starsEarned > 0 ? unlockedStar : lockedStar;
        star2.sprite = starsEarned > 1 ? unlockedStar : lockedStar;
        star3.sprite = starsEarned > 2 ? unlockedStar : lockedStar;
        
    }

    private void CreateFaceDictionary() {
        endFaces = new Dictionary<int, Sprite>();
        endFaces.Add(EndReason.drowned, drownedAl);
        endFaces.Add(EndReason.explode, explodedAl);
        endFaces.Add(EndReason.passed, passedAl);
        endFaces.Add(EndReason.timeout, timeoutAl);
        endFaces.Add(EndReason.winner, winnerAl);
		endFaces.Add (EndReason.lose, loserAl);
    }

    public void PressRestart() {
        Application.LoadLevel(level.name);
    }

    public void PressLevelMap() {
        Application.LoadLevel("LevelMap");
    }

}
