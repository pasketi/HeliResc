using UnityEngine;
using System.Collections;

[System.Serializable]
public class CargoSpace : Upgradable {

    public int maxCapacity;                                                 //Maximum amount of cargo to fit in the copter
    public int currentCargo;                                                //Current amount of cargo space used
    public int CurrentCargo { get { return currentCargo; } }                //Getter for current cargo
    public bool CargoFull { get { return currentCargo >= maxCapacity; } }   //Boolean to check if cargo is full

    private LevelManager manager;

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
        playerRb.mass = cargoMass + hookMass + copterMass; 
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

    public void CargoHookedCrates(GameObject hook)
    {
        foreach (Transform child in hook.transform)
        {

            if (child.FindChild("LegHook") != null && child.FindChild("LegHook").childCount != 0)
                CargoHookedCrates(child.FindChild("LegHook").gameObject);
            if (maxCapacity > currentCargo)
            {
                if (child.tag == "Crate")
                {
                    ChangeCargoMass(child.GetComponentInChildren<CrateManager>().crateMass);
                    ChangeHookMass(-child.GetComponentInChildren<CrateManager>().crateMass);
                    AddItemToCargo();
                    child.GetComponentInChildren<CrateManager>().inCargo = true;
                    child.parent = playerRb.transform;
                    child.GetComponent<Collider2D>().enabled = false;
                    child.GetComponent<Renderer>().enabled = false;
                    child.GetComponent<Rigidbody2D>().isKinematic = true;
                    child.transform.localPosition = Vector3.zero;
                    manager.setCargoCrates(currentCargo);
                }
            }
            else { /*Fix the weird physics here*/ }
        }
    }
    public void UnloadAll() {
        manager.saveCrates(currentCargo);
        currentCargo = 0;

        cargoMass = 0;
        playerRb.mass = cargoMass + hookMass + copterMass;
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
