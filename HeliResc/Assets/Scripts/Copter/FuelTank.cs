using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class FuelTank : Upgradable {

    public float maxCapacity = 100;                 //Maximum amount of fuel the player can have
    public float spendingRate = 1;                  //How much fuel is used
    public float fillRate = 40;                     //how much the tank is filled per second
    public float currentFuel;                       //The current amount of fuel    
    public float CurrentFuel { get { return currentFuel; } }
    public float CurrentFuelPercentage { get { return currentFuel / maxCapacity; } }

    private bool useFuel = true;        //Should the copter spend fuel
    private bool fill = false;          //Fill the tank until it's full

    public Action TankDepleted = () => { };
    public Action TankFilled = () => { };

    public override void Init(Copter copter) {
        base.Init(copter);
        
        UpdateDelegate = UseFuel;    //Set the update method
        currentFuel = maxCapacity;
    }

    public override void Upgrade() {
        throw new System.NotImplementedException();
    }
    public void Kinematic(bool k) {
        if (k == true)
            UpdateDelegate = () => { };
        else
            UpdateDelegate = UseFuel;
    }
    public void FillTank() {
        UpdateDelegate = FillFuel;
        fill = true;
        useFuel = false;
		playerCopter.SetInputActive(true);
		GameObject.Find("LevelManagerO").GetComponent<LevelManager>().setResetButton(false);
    }

    /// <summary>
    /// Depletes the tank
    /// </summary>
    private void UseFuel() {
        currentFuel -= spendingRate * playerCopter.engine.CurrentPower * Time.deltaTime;
        if (currentFuel <= 0f) {
            useFuel = false;                            //Disable use of fuel when the tank is empty
            TankDepleted();                             //Trigger an event to notify the tank is empty
			GameObject.Find("LevelManagerO").GetComponent<LevelManager>().setResetButton(true);
        }
    }
    private void FillFuel() {
        currentFuel += fillRate * Time.deltaTime;       //Add fuel to tank
        TankFilled();
        if (currentFuel > maxCapacity) {                //Stop fill if the tank is already full
            currentFuel = maxCapacity;
            fill = false;
            useFuel = true;                             //Start to use fuel again
            UpdateDelegate = UseFuel;
        }
    }
    protected override void GiveName() {
        name = "FuelTank";
    }
}
