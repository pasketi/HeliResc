using UnityEngine;
using System.Collections;

[System.Serializable]
public class Upgradable {

    public bool allowUpgrade;
    public string name;
    protected Rigidbody2D playerRb;
    protected Copter playerCopter;

    public void Init(Copter copter) {
        playerRb = copter.GetComponent<Rigidbody2D>();
        playerCopter = copter;
        Debug.Log("Upgrade added: " + name);
    }

    //Updates every frame
    public virtual void Update() { 
    
    }

    //Update every frame when the player holds his finger on the screen
    public virtual void InputUpdate(Touch touch) { 
        
    }
    //Update when the touchphase is Began
    public virtual void TouchStart(Touch touch) { }
    public virtual void TouchEnd(Touch touch) { }

    public virtual void Upgrade() { 
    
    }
}
