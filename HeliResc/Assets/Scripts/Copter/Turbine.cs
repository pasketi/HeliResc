using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Turbine : Upgradable {

	public float horizontalForce;
	public GameObject turboButtonPrefab;

	public override void Init (Copter copter) {
		base.Init (copter);
	}


	#region implemented abstract members of Upgradable

	protected override void GiveName ()
	{
		name = "Turbine";
	}

	#endregion



}
