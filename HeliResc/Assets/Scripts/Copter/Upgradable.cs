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

    public virtual void Update() { 
    
    }

    public virtual void InputUpdate(Touch touch) { 
        
    }

    public virtual void Upgrade() { 
    
    }
}
