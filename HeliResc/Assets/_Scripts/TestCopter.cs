using UnityEngine;
using System.Collections;

public class TestCopter : Copter {

    public CopterWaterManager waterManager;

    protected override void AddUpgradables() {
        base.AddUpgradables();
        waterManager.Init(this);
    }
}
