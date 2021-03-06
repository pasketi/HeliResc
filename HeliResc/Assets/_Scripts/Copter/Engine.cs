using UnityEngine;
using System.Collections;

[System.Serializable]
public class Engine : Upgradable {

    public float powerSensitivity;
    public float rotationSensitivity;

    public float powerMultiplier;    

    //rotation related variables
    public float currentAngle;
    public float copterAngle = 0;
    public float maxTiltSpeed = 75;
    public float maxTiltValue = 100;
    public float returnSpeed = 5;  //Returns the copter back to 0 angle

    public float holdTime = 0.25f;  //Time to limit the return speed
    private float tempHoldTime = 0; //holder to calculate the time passed since player let go of the screen
    private float persistence = 1;  //Limits the return speed
    private float overAngleTime;

    private bool hasFuel = true;
    private bool hasInput;
    private bool useAutoHoover = true;

	private Transform copterTr;

    //power related variables
    public float minPower;
    public float currentPower;
    public float maxPower;
    [Range(0,1)]
    public float snapToPlatformPower = 0.85f;                       //How hard the copter snaps to platform when entering its trigger. 
    public float CurrentPower { get { return currentPower; } }
    public float CurrentPowerPercentage { get { return currentPower / maxPower; } }

    public override void RegisterListeners() {
        EventManager.StartListening(SaveStrings.eEnterPlatform, EnterPlatform);
        EventManager.StartListening(SaveStrings.eExitPlatform, ExitPlatform);
    }
    public override void UnregisterListeners() {
        EventManager.StopListening(SaveStrings.eEnterPlatform, EnterPlatform);
        EventManager.StopListening(SaveStrings.eExitPlatform, ExitPlatform);
    }

    public override void Init(Copter copter) {
        base.Init(copter);
        
		copterTr = playerRb.transform;
        copter.fuelTank.TankDepleted += OutOfFuel;      //Subscribe to the fuel tanks TankEmpty event
        copter.fuelTank.TankFilled += TankFilled;
        UpdateDelegate = FuelUpdate;                    //Start with having fuel

        tempHoldTime = holdTime;
        if (PlayerPrefs.HasKey(SaveStrings.sAutoHoover))
            useAutoHoover = PlayerPrefsExt.GetBool(SaveStrings.sAutoHoover);
        else
            useAutoHoover = true;
    }

    public override void Upgrade() {
        throw new System.NotImplementedException();
    }
    public void PlatformUpdate() {
		copterAngle = copterTr.eulerAngles.z;
        Thrust();
        SnapReturnRotation();
    }
    public void FuelUpdate() {
		copterAngle = copterTr.eulerAngles.z;        
        //Flip the copter depending on its angle
        if (playerRb.velocity.x > 0.1f)
        {
            playerCopter.Direction(true);   //True means to turn right
        }
        else if (playerRb.velocity.x < -0.1f)
        {
            playerCopter.Direction(false);
        }
        Thrust();                           //Move the copter
        SnapReturnRotation();
    }
    public void NoFuelUpdate() {
		copterAngle = copterTr.eulerAngles.z;
        if (currentPower > 0f) {
            currentPower -= currentPower * (currentPower / maxPower) * Time.deltaTime * 3f;
        } else {
            currentPower = 0;
        }
        Thrust();
    }

    public override void TouchStart(MouseTouch touch) {        
        currentAngle = copterAngle;
        hasInput = true;
    }
    public override void TouchEnd(MouseTouch touch) {
        tempHoldTime = 0;       //Reset tempholdtime
        hasInput = false;
    }

    public override void InputUpdate(MouseTouch touch) {
        HandleRotation(touch);
        HandlePower(touch);
        
    }

    private void EnterPlatform() {

        UpdateDelegate = PlatformUpdate;
        if (hasInput == true) return; //Do not snap if there is player input
        //currentPower *= snapToPlatformPower;
        
    }
    private void ExitPlatform() {
        UpdateDelegate = FuelUpdate;
    }

    private void AutoHoover() {        
        Vector2 vel = playerRb.velocity;
        if (!Mathf.Approximately(vel.y, 0)) {
            float y = 0.25f * Mathf.Abs(vel.y);
            y = Mathf.Clamp(y, 0.15f, 1);
            currentPower -= Mathf.Sign(vel.y) * (y);    //Depending on the direction of copter velocity add more power or lower it
        }
    }

    private void OutOfFuel() {
        UpdateDelegate = NoFuelUpdate;      //Change the update method
        hasFuel = false;
    }
    private void TankFilled() {
        hasFuel = true;
        if (playerCopter.OnPlatform == true) UpdateDelegate = PlatformUpdate;
        else UpdateDelegate = FuelUpdate;
    }

    private void Thrust() {
		playerRb.AddForce(copterTr.up * (currentPower * powerMultiplier) * Time.deltaTime);
    }

    public void IdleInput() {
        //If there is no input, autohoover and return the rotation back to 0
        if (hasFuel == true && useAutoHoover == true) {
            AutoHoover();
            IdleRotation();
        } 
    }

    private void HandlePower(MouseTouch touch) {
        if (currentPower <= maxPower && currentPower >= minPower)
        {
            if (touch.deltaTime != 0f) currentPower += (((touch.deltaPosition.y / Screen.height) * (maxPower - minPower)) * powerSensitivity) * (Time.deltaTime / touch.deltaTime);
            else currentPower += (((touch.deltaPosition.y / Screen.height) * (maxPower - minPower)) * powerSensitivity);
        }
        if (currentPower < minPower) currentPower = minPower;
        if (currentPower > maxPower) currentPower = maxPower;        
    }

    private void SnapReturnRotation() {
        if ((copterAngle > maxTiltValue + 5 && copterAngle <= 180) || (copterAngle > 180 && copterAngle < (355 - maxTiltValue)))
        {
            overAngleTime += Time.deltaTime;
        }
        else
        {
            overAngleTime = 0;
        }
        if (overAngleTime > 2)
        {
            overAngleTime = 0;
            copterTr.eulerAngles = Vector3.zero;
        }
    }

    private void HandleRotation(MouseTouch touch) {                

		if ((currentAngle < maxTiltValue
			|| touch.deltaPosition.x < 0f)
			|| (currentAngle > 360f - maxTiltValue
			|| touch.deltaPosition.x > 0f)) {
			currentAngle -= touch.deltaPosition.x * rotationSensitivity;
		}

		if (currentAngle < 0f) {
			currentAngle += 360f;
		} else if (currentAngle > 360f) {
			currentAngle -= 360f;
		}

		if (currentAngle > maxTiltValue && currentAngle < 180f) {
			currentAngle = maxTiltValue;
		} else if (currentAngle < 360f - maxTiltValue && currentAngle > 180f) {
			currentAngle = 360f - maxTiltValue;
		}

		Quaternion q = Quaternion.AngleAxis (currentAngle, Vector3.forward);

		copterTr.rotation = Quaternion.RotateTowards (copterTr.rotation, q, Time.deltaTime * maxTiltSpeed);
//        if (copterAngle != currentAngle)
//        { // Turn to currentAngle
//            if (currentAngle < 180f)
//            {
//                if (copterAngle > 180f)
//                {
//                    // Rotate CCW
//					//copterTr.Rotate(new Vector3(0f, 0f, maxTiltSpeed * Time.deltaTime));
//                }
//                else if (copterAngle < 180f)
//                {
//                    if (copterAngle < currentAngle)
//                    {
//                        // Rotate CCW
//						//copterTr.Rotate(new Vector3(0f, 0f, maxTiltSpeed * Time.deltaTime));
//                    }
//                    else if (copterAngle > currentAngle)
//                    {
//                        // Rotate CW
//						//copterTr.Rotate(new Vector3(0f, 0f, -maxTiltSpeed * Time.deltaTime));
//                    }
//                }
//            }
//            else if (currentAngle > 180f)
//            {
//                if (copterAngle < 180f)
//                {
//                    // Rotate CW
//					//copterTr.Rotate(new Vector3(0f, 0f, -maxTiltSpeed * Time.deltaTime));
//                }
//                else if (copterAngle > 180f)
//                {
//                    if (copterAngle < currentAngle)
//                    {
//                        // Rotate CCW
//						//copterTr.Rotate(new Vector3(0f, 0f, maxTiltSpeed * Time.deltaTime));
//                    }
//                    else if (copterAngle > currentAngle)
//                    {
//                        // Rotate CW
//						//copterTr.Rotate(new Vector3(0f, 0f, -maxTiltSpeed * Time.deltaTime));
//                    }
//                }
//            }
//		}        
    }

    private void IdleRotation() {
		//Debug.Log ("Idle rotation");
        if (copterAngle != 0f)
        { // Return to 0°
            if (copterAngle > 180f)
            {
				copterTr.Rotate(new Vector3(0f, 0f, returnSpeed * Time.deltaTime * (360f - copterAngle)));
            }
            else if (copterAngle < 180f)
            {
				copterTr.Rotate(new Vector3(0f, 0f, -(returnSpeed * Time.deltaTime) * copterAngle));
            }
        }
        //if (tempHoldTime != holdTime)
        //{
        //    tempHoldTime += Time.deltaTime;
        //    persistence = tempHoldTime / holdTime;
        //}
        //if (tempHoldTime > holdTime)
        //{
        //    persistence = 1f;
        //    tempHoldTime = holdTime;
        //}        
    }
    protected override void GiveName() {
        name = "Engine";
    }
}
