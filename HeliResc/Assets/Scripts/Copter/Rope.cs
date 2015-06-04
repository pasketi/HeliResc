using UnityEngine;
using System.Collections;

[System.Serializable]
public class Rope : Upgradable {

    public int durability;
    public float reelSpeed;

    public override void Init(Copter copter) {
        base.Init(copter);

        UpdateDelegate = RopeUpdate;
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public override void TouchStart(MouseTouch touch) {
        
    }

    public void RopeUpdate() { 
        
    }
}
