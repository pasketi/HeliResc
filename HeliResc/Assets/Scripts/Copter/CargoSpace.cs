using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CargoSpace : Upgradable {

    public int maxCapacity;                                                 //Maximum amount of cargo to fit in the copter
    public int currentCargo;                                                //Current amount of cargo space used
    public int CurrentCargo { get { return currentCargo; } }                //Getter for current cargo
    public int SpaceInCargo { get { return maxCapacity - currentCargo; } }  //How much space in the cargo
    public bool CargoFull { get { return currentCargo >= maxCapacity; } }   //Boolean to check if cargo is full

    private LevelManager manager;
    private int cargoValue;                                                 //The value of the items combined that are in the cargo

    public float copterMass;
    private float hookMass;
    private float cargoMass;

    public override void RegisterListeners() {
        EventManager.StartListening("EnterPlatform", UnloadAll);        
    }
    public override void UnregisterListeners() {
        EventManager.StopListening("EnterPlatform", UnloadAll);        
    }

    public override void Init(Copter copter) {
        base.Init(copter);
        
        manager = copter.levelManager;
        //playerRb.mass = cargoMass + hookMass + copterMass; 
    }

    public override void Upgrade() {
        throw new System.NotImplementedException();
    }


    /// <summary>
    /// Adds and item to cargo if there is room
    /// </summary>
    /// <param name="size"></param>
    /// <returns>true if the task was successfull</returns>
    public bool AddItemToCargo(int size = 1) {
        if (currentCargo < maxCapacity) {
            currentCargo += size;
            return true;
        }
        else
            return false;
    }

    public List<SaveableObject> CargoHookedCrates(List<SaveableObject> savedObjects) {
        if (savedObjects.Count <= 0) return new List<SaveableObject>();
        while (currentCargo < maxCapacity && savedObjects.Count > 0) {
            AddItemToCargo();
            cargoValue += savedObjects[0].saveValue;
            savedObjects[0].CargoItem();
            savedObjects.RemoveAt(0);
        }
        manager.setCargoCrates(currentCargo);
        if (savedObjects.Count > 0)
            return savedObjects;
        else
            return new List<SaveableObject>();
    }

    public void CargoHookedCrates(Transform hookChild)
    {

        Debug.Log("cargo: " + currentCargo);
        if (hookChild.FindChild("LegHook") != null && hookChild.FindChild("LegHook").childCount != 0)
            CargoHookedCrates(hookChild.FindChild("LegHook").gameObject.transform);
        if (maxCapacity >= currentCargo)
        {
            if (hookChild.tag == "Crate")
            {
                ChangeCargoMass(hookChild.GetComponentInChildren<CrateManager>().crateMass);
                ChangeHookMass(-hookChild.GetComponentInChildren<CrateManager>().crateMass);
                AddItemToCargo();
                hookChild.GetComponentInChildren<CrateManager>().inCargo = true;
                hookChild.parent = playerRb.transform;
                hookChild.GetComponent<Collider2D>().enabled = false;
                hookChild.GetComponent<Renderer>().enabled = false;
                hookChild.GetComponent<Rigidbody2D>().isKinematic = true;
                hookChild.transform.localPosition = Vector3.zero;
                manager.setCargoCrates(currentCargo);
            }
        }
        else { /*Fix the weird physics here*/ }
    }
    public void UnloadAll() {
        manager.saveCrates(currentCargo);
        currentCargo = 0;

        cargoMass = 0;
        //playerRb.mass = cargoMass + hookMass + copterMass;
        manager.setCargoCrates(currentCargo);
    }    

    public void ChangeCargoMass(float crateMass)
    {
        cargoMass += crateMass;
        playerRb.mass = cargoMass + hookMass + copterMass;
    }

    public void ChangeHookMass(float crateMass)
    {
        hookMass += crateMass;
        playerRb.mass = cargoMass + hookMass + copterMass;
    }
    public void saveHookedCrate(float crateMass)
    {
        manager.saveCrates(1);
        ChangeHookMass(-crateMass);
    }
    protected override void GiveName() {
        name = "CargoSpace";
    }
}
