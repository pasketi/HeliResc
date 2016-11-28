using UnityEngine;
using System.Collections;

public class AirshipScript : Copter {

	public Turbine turbine;
	public CopterFloats floats;
	public CopterWaterManager waterManager;

	protected override void AddUpgradables () {
		base.AddUpgradables ();
		turbine.Init (this);
		waterManager.Init (this);
		floats.Init(this);
	}
}
