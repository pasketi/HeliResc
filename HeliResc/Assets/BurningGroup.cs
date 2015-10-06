using UnityEngine;
using System.Collections;

public class BurningGroup : ActionableObject {

    private BurningObject[] objects;                    //All burning objects that are children of this gameobject
    private LevelManager manager;                       //Reference to level manager

    private bool saved;                                 //True if all the objects's fire has been put out
    private int currentlySaved;                         //Number of burning objects saved
    private int objCount;                               //Lenght of objects array

    void Awake() {
        manager = GameObject.FindObjectOfType<LevelManager>();
        objects = transform.GetComponentsInChildren<BurningObject>();
        objCount = objects.Length;
        currentlySaved = 0;
    }

    public override void UseAction()
    {
        if (saved == true) return;

        currentlySaved = 0;

        //Check how many objects are burning
        foreach (BurningObject bo in objects) {
            if (bo.burning == false)
                currentlySaved++;
        }
        //If none then mark the group as saved
        if (currentlySaved >= objCount) {
            saved = true;
            manager.saveCrates(1);
        }
    }
}
