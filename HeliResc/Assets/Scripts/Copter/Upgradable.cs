using UnityEngine;
using System.Collections;

[System.Serializable]
public class Upgradable {

    public bool allowUpgrade;                           //Should this item be able to be upgraded
    public string name;                                 //Identifying name of the object
    protected Rigidbody2D playerRb;                     //Reference to player rigidbody
    protected Copter playerCopter;                      //Reference to Copter script of the current copter

    public delegate void UpdateMethod();
    public UpdateMethod UpdateDelegate = () => { };

    //Initializes the required members of the object
    public virtual void Init(Copter copter) {
        playerRb = copter.GetComponent<Rigidbody2D>();  //Get the reference to players rigidbody
        playerCopter = copter;
        playerCopter.AddToDictionary(this);             //Add the new Upgrade to the copters upgrade list
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
    //Upgrade the object
    public virtual void Upgrade() { 
    
    }
}
