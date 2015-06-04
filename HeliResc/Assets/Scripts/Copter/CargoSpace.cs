using UnityEngine;
using System.Collections;

[System.Serializable]
public class CargoSpace : Upgradable {

    public int maxCapacity;                 //Maximum amount of cargo to fit in the copter
    private int currentCargo;               //Current amount of cargo space used

    public void Init(Copter copter) {
        base.Init(copter);
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
}
