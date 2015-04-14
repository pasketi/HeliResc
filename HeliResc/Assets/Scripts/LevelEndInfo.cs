using UnityEngine;
using System.Collections;

public class LevelInfo
{
    public int index;

    public bool star1;
    public bool star2;
    public bool star3;
    public bool locked;

    public LevelInfo() {

    }

    public LevelInfo(int i, bool s1, bool s2, bool s3, bool l)
    {
        index = i;

        star1 = s1;
        star2 = s2;
        star3 = s3;

        locked = l;
    }
}

public class LevelEndInfo : LevelInfo {


	public string star1Objective;
	public string star2Objective;
	public string star3Objective;

	public string message;

	public int collectedCoins;
	public int itemsSaved;

	public LevelEndInfo(bool passed) {
        locked = !passed;
        SetStars();
	}

	public void SetStars(bool s1 = false, bool s2 = false, bool s3 = false) {
		star1 = s1;
		star2 = s2;
		star3 = s3;
	}
}
