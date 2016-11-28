using UnityEngine;
using System.Collections;

public class WaterCopter : Copter {

    public CopterFloats floats;

    protected override void AddUpgradables() {
        base.AddUpgradables();
        floats.Init(this);
    }
}
