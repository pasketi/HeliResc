using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MissionObjectives : MonoBehaviour {

    public delegate bool LevelObjective();

    public Action OnGameOver = () => { };

    public Objective Objective1;
    public Objective Objective2;
    public Objective Objective3;

    public LevelObjective LevelObjective1;
    public LevelObjective LevelObjective2;
    public LevelObjective LevelObjective3;

    private LevelManager manager;

    //Dictionary for all the different objectives.
    private Dictionary<Objective, LevelObjective> objectiveMethods;

    void Start() {
        manager = GameObject.FindObjectOfType<LevelManager>();

        objectiveMethods = new Dictionary<Objective, LevelObjective>();
        objectiveMethods.Add(Objective.GetItems, GetItems);
        objectiveMethods.Add(Objective.PassLevel, PassLevel);
        objectiveMethods.Add(Objective.Time, TimeChallenge);

        LevelObjective1 = objectiveMethods[Objective1];
        LevelObjective2 = objectiveMethods[Objective2];
        LevelObjective3 = objectiveMethods[Objective3];

    }    

    public bool GetItems() {
		return manager.allCratesCollected ();
    }

    public bool PassLevel() {
        return true;
    }

    public bool TimeChallenge() {
        return LevelHandler.CurrentLevel.levelTimeChallenge > manager.LevelTimer && GetItems();
    }

    public LevelObjective GetMethod(Objective o) {
        return objectiveMethods[o];
    }

    public bool AnyObjectiveCompleted() {
        return (LevelObjective1() || LevelObjective2() || LevelObjective3());
    }

    public bool AllObjectiveCompleted() {
        return (LevelObjective1() && LevelObjective2() && LevelObjective3());
    }
}
public enum Objective {
    GetItems,
    PassLevel,
    Time
}