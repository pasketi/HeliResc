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

    //Dictionary for all the different objectives.
    private Dictionary<Objective, LevelObjective> objectiveMethods;

    void Start() {
        objectiveMethods = new Dictionary<Objective, LevelObjective>();
        objectiveMethods.Add(Objective.GetItems, GetItems);
        objectiveMethods.Add(Objective.NoDamage, NoDamage);
        objectiveMethods.Add(Objective.PassLevel, PassLevel);
        objectiveMethods.Add(Objective.HitTrigger, HitTrigger);
        objectiveMethods.Add(Objective.Tutorial, Tutorial);

        LevelObjective1 = objectiveMethods[Objective1];
        LevelObjective2 = objectiveMethods[Objective2];
        LevelObjective3 = objectiveMethods[Objective3];

    }    

    public bool GetItems()
    {
        //TODO
        Debug.Log("Get Items");
        return false;
    }

    public bool NoDamage()
    {
        //TODO
        Debug.Log("No Damage");
        return false;
    }

    public bool PassLevel() {
        return true;
    }

    public bool HitTrigger() {
        ObjectiveTrigger[] triggers = GameObject.FindObjectsOfType<ObjectiveTrigger>();
        if (triggers.Length == 1)
            return triggers[0].triggered;
        return false;
    }

    public bool Tutorial() {
        TutorialScript t = GameObject.FindObjectOfType<TutorialScript>();
        return t.FuelRepair;
    }

    public LevelObjective GetMethod(Objective o) {
        return objectiveMethods[o];
    }
}
public enum Objective {
    GetItems,
    NoDamage,
    PassLevel,
    HitTrigger,
    Tutorial
}