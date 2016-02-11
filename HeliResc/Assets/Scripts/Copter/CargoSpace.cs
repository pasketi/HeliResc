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

    private List<HookableObject> cargoItems;

    protected float copterMass;
    protected float hookMass;
    protected float cargoMass;

    public override void RegisterListeners() {
        EventManager.StartListening(SaveStrings.eEnterPlatform, UnloadAll);        
    }
    public override void UnregisterListeners() {
        EventManager.StopListening(SaveStrings.eEnterPlatform, UnloadAll);        
    }

    public override void Init(Copter copter) {
        base.Init(copter);
        cargoItems = new List<HookableObject>();
        manager = copter.levelManager;
        copterMass = playerRb.mass;
    }

    public override void Upgrade() {
        throw new System.NotImplementedException();
    }


    /// <summary>
    /// Adds and item to cargo if there is room
    /// </summary>
    /// <param name="size"></param>
    /// <returns>true if the task was successfull</returns>
    public bool AddItemToCargo(HookableObject item) {
        if (currentCargo+item.size <= maxCapacity) {
            cargoItems.Add(item);
            currentCargo += item.size;
            cargoMass += item.mass;
            playerRb.mass = copterMass + cargoMass;
            return true;
        }
        else
            return false;
    }

    public List<HookableObject> CargoHookedCrates(List<HookableObject> savedObjects) {
        if (savedObjects.Count <= 0) return new List<HookableObject>();        
        while (currentCargo < maxCapacity && savedObjects.Count > 0) {            
            AddItemToCargo(savedObjects[0]);            //Add the item to the dictionary      
            cargoValue += savedObjects[0].saveValue;    //Get the save value from the crate
            savedObjects[0].CargoItem();                //Move item to cargo            
        }
        manager.setCargoCrates(currentCargo);
        if (savedObjects.Count > 0)
            return savedObjects;
        else
            return new List<HookableObject>();
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
                //ChangeHookMass(-hookChild.GetComponentInChildren<CrateManager>().crateMass);
                //AddItemToCargo();
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
        foreach (HookableObject ho in cargoItems) {
            if (ho.saveable == true) { 
                manager.saveCrates(1); 
            }
        }
        
        currentCargo = 0;
        cargoItems = new List<HookableObject>();

        cargoMass = 0;
        playerRb.mass = cargoMass + copterMass;
        manager.setCargoCrates(currentCargo);
    }    

    public void ChangeCargoMass(float crateMass)
    {
        cargoMass += crateMass;
        playerRb.mass = cargoMass + copterMass;
    }
       
    protected override void GiveName() {
        name = "CargoSpace";
    }
}
