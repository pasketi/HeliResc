using UnityEngine;
using System.Collections;

public class MiniCopter : Copter {
	public CopterWaterManager waterManager;

	protected override void AddUpgradables() {
		base.AddUpgradables();
		waterManager.Init(this);
	}

}
