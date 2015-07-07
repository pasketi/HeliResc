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

    public LevelInfo(int i, bool l) {
        index = i;
        locked = l;
    }

    public LevelInfo(int i, bool s1, bool s2, bool s3, bool l)
    {
        index = i;

        star1 = s1;
        star2 = s2;
        star3 = s3;

        locked = l;
    }

    public void SetStars(bool s1 = false, bool s2 = false, bool s3 = false)
    {
        star1 = s1;
        star2 = s2;
        star3 = s3;
    }
}

public class LevelEndInfo : LevelInfo {


    public bool obj1Passed;
    public bool obj2Passed;
    public bool obj3Passed;

	public int endCondition;

    public bool passedLevel;

    private int reward;
    public int Reward { set { reward = value; collectedCoins = value * itemsSaved; } get { return reward; } }

	public int collectedCoins;
	public int itemsSaved;

	public LevelEndInfo(bool passed, int condition) {
        passedLevel = passed;
        endCondition = condition;
        SetStars();
	}
}

public class EndReason {
    //This class is to enable auto complete
    public static int winner = 1;
    public static int passed = 2;
    public static int drowned = 3;
    public static int explode = 4;
    public static int timeout = 5;
}
