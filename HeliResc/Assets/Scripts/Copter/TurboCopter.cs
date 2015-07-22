using UnityEngine;
using System.Collections;

public class TurboCopter : Copter {

	public Turbine turbine;
	public CopterWaterManager waterManager;

	protected override void AddUpgradables () {
		base.AddUpgradables ();
		turbine.Init (this);
		waterManager.Init (this);
	}
}
