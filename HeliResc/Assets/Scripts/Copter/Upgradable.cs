using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Upgradable {

    public bool allowUpgrade;                           //Should this item be able to be upgraded
    protected string name;                              //Identifying name of the object
    protected Rigidbody2D playerRb;                     //Reference to player rigidbody
    protected Copter playerCopter;                      //Reference to Copter script of the current copter

    public Upgrade upgrade;                             //Components upgrade information
    public string Name { get { return name; } }

    public delegate void UpdateMethod();
    public UpdateMethod UpdateDelegate = () => { };     //Delegate method to run in update. Replace with the method that should run as update

    //Initializes the required members of the object
    public virtual void Init(Copter copter) {
        GiveName();
        playerRb = copter.GetComponent<Rigidbody2D>();  //Get the reference to players rigidbody
        playerCopter = copter;
        playerCopter.AddToDictionary(this);             //Add the new Upgrade to the copters upgrade list

        upgrade.Init(playerCopter.name + name);
    }

    #region Update methods
    //Updates every frame
    public void Update() { UpdateDelegate(); }
    //Update every frame when the player holds his finger on the screen
    public virtual void InputUpdate(MouseTouch touch) { }
    //Update when the touchphase is Began
    public virtual void TouchStart(MouseTouch touch) { }
    //Update when the touchphase is Ended
    public virtual void TouchEnd(MouseTouch touch) { }
    #endregion

    #region Event methods
    public virtual void RegisterListeners() { }
    public virtual void UnregisterListeners() { }
    #endregion
    //Upgrade the object
    public virtual void Upgrade() { 
    
    }
    protected abstract void GiveName();
}
