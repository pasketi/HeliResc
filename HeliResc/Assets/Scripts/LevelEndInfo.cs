using UnityEngine;
using System.Collections;

public class LevelEndInfo {

	public bool star1;
	public bool star2;
	public bool star3;

	public string star1Objective;
	public string star2Objective;
	public string star3Objective;

	public bool passed;

	public string message;

	public int collectedCoins;
	public int itemsSaved;

	public LevelEndInfo() {
		star1 = true;
	}

	public void SetStars(bool s1 = false, bool s2 = false, bool s3 = false) {
		star1 = s1;
		star2 = s2;
		star3 = s3;
	}
}
