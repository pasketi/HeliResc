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
		objectiveMethods.Add(Objective.ObjectiveNotImplemented, objectiveNotImplemented);
		objectiveMethods.Add(Objective.WhaleSeen, PassLevel);
		objectiveMethods.Add(Objective.Refueled, isCopterRefueled);
		objectiveMethods.Add(Objective.DiverInCargo, isDiverInCargo);
		objectiveMethods.Add(Objective.DryCat, isCatDry);
		objectiveMethods.Add(Objective.MultipleCratesHooked, multipleCratesHooked);
		objectiveMethods.Add(Objective.NoThrowsMissed, noMissedThrows);
		objectiveMethods.Add(Objective.ChainFishermen, fishermenChained);
		objectiveMethods.Add(Objective.shipNotHit, shipNotHit);


		LevelObjective1 = objectiveMethods[Objective1];
		LevelObjective2 = objectiveMethods[Objective2];
		LevelObjective3 = objectiveMethods[Objective3];
    }

	public void refreshLevelObjectives(){
		LevelObjective1 = objectiveMethods[Objective1];
		LevelObjective2 = objectiveMethods[Objective2];
		LevelObjective3 = objectiveMethods[Objective3];
	}

	public bool objectiveNotImplemented () {
		return false;
	}

    public bool GetItems() {
		return manager.allCratesCollected();
    }

    public bool PassLevel() {
        return true;
    }

    public bool TimeChallenge() {
        return LevelHandler.CurrentLevel.levelTimeChallenge > manager.LevelTimer && GetItems();
    }

	public bool isCopterRefueled() {
		return manager.isCopterRefueled;
	}

	public bool isDiverInCargo() {
		return manager.isDiverInCargo;
	}

	public bool isCatDry() {
		return manager.isCatDry;
	}

	public bool multipleCratesHooked() {
		return manager.multipleCratesHooked;
	}

	public bool noMissedThrows () {
		bool missed = false;
		foreach (GameObject test in GameObject.FindObjectsOfType<GameObject>()) {
			if (!missed) missed = !test.name.Contains("LifeRing");
		}
		return missed;
	}

	public bool fishermenChained () {
		return manager.chainFormed;
	}

	public bool shipNotHit () {
		return manager.shipNotHit;
	}

    public LevelObjective GetMethod(Objective o) {
        return objectiveMethods[o];
    }

	public bool ObjectiveOneCompleted() {
		return LevelObjective1();
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
    Time,
	ObjectiveNotImplemented,
	WhaleSeen,
	Refueled,
	DiverInCargo,
	DryCat,
	MultipleCratesHooked,
	NoThrowsMissed,
	ChainFishermen,
	shipNotHit
}