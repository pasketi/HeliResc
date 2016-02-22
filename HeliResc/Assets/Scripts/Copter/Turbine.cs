using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Turbine : Upgradable {

	public bool createSwitchButton;			//Should the copter instantiate the switch button
	public float horizontalForce;			//How much force to apply to the copter
	public GameObject turboButtonPrefab;	//The UI button to place on the screen
	public GameObject switchButtonPrefab;	//Switch button to place on the screen

	protected Rect buttonRectangle;			//Rectangle of the turbo button in world space
	protected bool pressingButton;			//is the player currently pressing the turbo button
	protected float direction;				//Based on the copter x-scale the direction the copter is going

	public override void Init (Copter copter) {
		base.Init (copter);

		GameObject hud = GameObject.Find ("HUD");
		if (hud == null)
			return;

		if (createSwitchButton == true) {
			GameObject switchButton = copter.CreateGameObject (switchButtonPrefab, Vector3.zero, Quaternion.identity);
			switchButton.transform.SetParent (hud.transform);
			RectTransform swRect = switchButton.GetComponent<RectTransform> ();
			swRect.anchoredPosition = new Vector2 (Screen.width * 0.91f, Screen.height * 0.356f);
			swRect.sizeDelta = new Vector2 (Screen.width * 0.095f, Screen.height * 0.178f);
			swRect.localScale = Vector3.one;

			Button b = switchButton.GetComponent<Button> ();
			b.onClick.AddListener (() => SwitchDirection ());
		}

		GameObject button = copter.CreateGameObject (turboButtonPrefab, turboButtonPrefab.transform.position, Quaternion.identity);
		button.transform.SetParent (hud.transform);
		button.transform.SetSiblingIndex(0);
		RectTransform rect = button.transform as RectTransform;

		rect.anchoredPosition = new Vector2 (Screen.width * 0.91f, Screen.height * (0.19f/2f));
		rect.sizeDelta = new Vector2 (Screen.width * 0.15f, Screen.height * 0.18f);
		rect.localScale = Vector3.one;

		buttonRectangle = new Rect ();

		buttonRectangle.x = rect.anchoredPosition.x - rect.sizeDelta.x * 0.5f;
		buttonRectangle.width = rect.sizeDelta.x;

		buttonRectangle.y = rect.anchoredPosition.y - rect.sizeDelta.y * 0.5f;
		buttonRectangle.height = rect.sizeDelta.y;

		UpdateDelegate = TurbineUpdate;
	}
	
	public override void InputUpdate (MouseTouch touch) {
		if (buttonRectangle.Contains (touch.position)) {
			pressingButton = true;
		} else {
			pressingButton = false;
		}
	}

	protected void TurbineUpdate() {
		if (pressingButton == true) {
			direction = Mathf.Sign (playerRb.transform.localScale.x);
			Vector2 force = Vector2.right * direction * horizontalForce * Time.deltaTime;
			Debug.Log(force);
			playerRb.AddForce(force);
		}
		//pressingButton = false;
	}

	protected void SwitchDirection() {
		Debug.Log ("Switch");
		playerCopter.Direction(true);
	}

	#region implemented abstract members of Upgradable

	protected override void GiveName ()
	{
		name = "Turbine";
	}

	#endregion



}
