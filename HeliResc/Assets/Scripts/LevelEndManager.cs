using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelEndManager : MonoBehaviour {

	private GameManager gameManager;
	private MoneyUIUpdate moneyText;

    public Text debugTime;

	public Image star1, star2, star3;
	private Text star1Text, star2Text, star3Text;
	private Text targetTime, playerTime;
	private Image personalBest;
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
    public Sprite unlockedSapphire;

	public GameObject coinPrefab;

	private LevelEndInfo levelEnd;
    private Level level;

    private Animator animator;

    private int starsEarned;


    public void UpdateLevelEnd(GameManager gm) {
        gameManager = gm;
		moneyText = GameObject.Find("TextMoney").GetComponent<MoneyUIUpdate>();

		CreateFaceDictionary();

		star1Text = star1.GetComponentInChildren<Text>();
		star2Text = star2.GetComponentInChildren<Text>();
		star3Text = star3.GetComponentInChildren<Text>();
		targetTime = GameObject.Find("TargetTime").GetComponent<Text>();
		playerTime = GameObject.Find("YourTime").GetComponent<Text>();
		personalBest = GameObject.Find("PersonalBest").GetComponent<Image>();
		personalBest.gameObject.SetActive(false);

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
		moneyText.setOldMoney(gameManager.wallet.Coins);
		moneyText.setLevelEnd(true);

		if (level.star1 == false && levelEnd.obj1Passed == true) {
			gameManager.wallet.AddMoney(10 + 10 * level.id);
			StartCoroutine(CoinFlow(10+10*level.id, GameObject.Find("Star1")));
			playerStars++;
		}
		if (level.star2 == false && levelEnd.obj2Passed == true) {
			gameManager.wallet.AddMoney(10 + 10 * level.id);
			StartCoroutine(CoinFlow(10+10*level.id, GameObject.Find("Star2")));
			playerStars++;
		}
		if (level.star3 == false && levelEnd.obj3Passed == true) {
			gameManager.wallet.AddMoney(10 + 10 * level.id);
			StartCoroutine(CoinFlow(10+10*level.id, GameObject.Find("Star3")));
			playerStars++;
		}

		PlayerPrefs.SetInt (SaveStrings.sPlayerStars, playerStars);

		if (level.rubyFound == false && levelEnd.rubyFound == true) {
			int rubies = PlayerPrefs.GetInt(SaveStrings.sPlayerRubies) + 1;
			PlayerPrefs.SetInt(SaveStrings.sPlayerRubies, rubies);
		}

		star1Text.text = levelEnd.itemsSaved.ToString() + "/" + levelEnd.maxItems.ToString();
		star2Text.text = levelEnd.obj2Passed || level.star2 ? "Bonus" : "Bonus" ;

		float timeDifference = level.bestTime < level.levelTimeChallenge ? (levelEnd.levelTime - level.bestTime) : (levelEnd.levelTime - level.levelTimeChallenge) ;
		star3Text.text = (timeDifference.CompareTo(0f) >= 0 ? "+ " : "- ") + Mathf.Abs(timeDifference / 60f).ToString("#0") + ":" + Mathf.Abs(timeDifference % 60f).ToString("00.##");

		playerTime.text = "Time: " + (levelEnd.levelTime / 60f).ToString("#0") + ":" + (levelEnd.levelTime % 60f).ToString("00.##");

		if (level.bestTime > level.levelTimeChallenge) {
			targetTime.text = "Target: " + (level.levelTimeChallenge / 60f).ToString("#0") + ":" + (level.levelTimeChallenge % 60f).ToString("00.##");
			if (levelEnd.levelTime < level.levelTimeChallenge) {
				level.bestTime = levelEnd.levelTime;
				personalBest.gameObject.SetActive(true);
				PlayerPrefs.SetFloat(level.name + "BestTime", level.bestTime);
			}
		} else {
			targetTime.text = "Best: " + (level.bestTime / 60f).ToString("#0") + ":" + (level.bestTime % 60f).ToString("00.##");
			if (levelEnd.levelTime < level.bestTime) {
				level.bestTime = levelEnd.levelTime;
				personalBest.gameObject.SetActive(true);
				PlayerPrefs.SetFloat(level.name + "BestTime", level.bestTime);
			}
		}

		level.rubyFound = levelEnd.rubyFound || level.rubyFound;
        level.star1 = levelEnd.obj1Passed || level.star1;
        level.star2 = levelEnd.obj2Passed || level.star2;
        level.star3 = levelEnd.obj3Passed || level.star3;

		// Grafiikoiden määritys
        if (levelEnd.passedLevel == true)
        {            
            PassedLevel();
            Level.Save(level);
        }
        else
        {
            FailedLevel();
        }

        if (levelEnd.rubyFound == true)
        {
            rubyImage.sprite = levelEnd.sapphireFound ? unlockedSapphire : unlockedRuby;
        }
        else
        {
            rubyImage.enabled = false;
        }

        debugTime.text = "Time spent: " + levelEnd.levelTime;

        //StartCoroutine(Animations(levelEnd.rubyFound));		

    }

    private IEnumerator Animations(bool showRuby) {
        yield return null;
    }
		private IEnumerator CoinFlow(int amount, GameObject spawnPosition) {

		// Coin initialization
		yield return new WaitForSeconds(1f);
		GameObject[] coins = new GameObject[amount];
		//Debug.Log(coinDestination);
		for (int i = 0; i < amount; i++) {
			coins[i] = Instantiate(coinPrefab, spawnPosition.transform.position, Quaternion.identity) as GameObject;
			coins[i].transform.SetParent(spawnPosition.transform.parent.transform);
			Vector2 force = new Vector2 (Random.value-0.5f,Random.value-0.5f);
			force.Normalize();
			coins[i].GetComponent<Rigidbody2D>().AddForce(force * (Random.value * 7500f + 2500f), ForceMode2D.Force);
		}

		/*foreach (GameObject coin in coins) {
						//coin.transform.localScale = new Vector3(1f,1f,1f);
						//coin.GetComponent<RectTransform>().anchorMin = new Vector2 (0f,0f);
						//coin.GetComponent<RectTransform>().anchorMax = new Vector2 (0.1f,0.1f);
				}*/

		// End
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
		star1.sprite = level.star1 ? unlockedStar : lockedStar;
		star2.sprite = level.star2 ? unlockedStar : lockedStar;
		star3.sprite = level.star3 ? unlockedStar : lockedStar;
        
    }

    private void CreateFaceDictionary() {
        endFaces = new Dictionary<int, Sprite>();
        endFaces.Add(EndReason.drowned, drownedAl);
        endFaces.Add(EndReason.explode, explodedAl);
        endFaces.Add(EndReason.passed, passedAl);
        endFaces.Add(EndReason.timeout, timeoutAl);
        endFaces.Add(EndReason.winner, winnerAl);
		endFaces.Add(EndReason.lose, loserAl);
    }

    public void PressRestart() {
        GameManager.LoadLevel(level.name);
    }

    public void PressLevelMap() {
        GameManager.LoadLevel("LevelMap");
    }

}
