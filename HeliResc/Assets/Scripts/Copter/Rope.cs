using UnityEngine;
using System.Collections;

[System.Serializable]
public class Rope : Upgradable {

    public void Init(Copter copter) {
        base.Init(copter);
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }
}
