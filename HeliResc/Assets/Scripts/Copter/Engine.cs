using UnityEngine;
using System.Collections;

[System.Serializable]
public class Engine : Upgradable {

    public float powerSensitivity;
    public float rotationSensitivity;

    public float powerMultiplier;

    private FuelTank tank;

    //rotation related variables
    public float currentAngle;
    public float maxTiltSpeed = 75;
    public float maxTiltValue = 100;

    //power related variables
    public float minPower;
    public float currentPower;
    public float maxPower;    

    public void Init(Copter copter) {
        base.Init(copter);
        tank = copter.fuelTank;
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        Thrust();
    }

    public override void InputUpdate(Touch touch) {
        Debug.Log("Copter engine update");
        HandleRotation(touch);
        HandlePower(touch);
        
    }

    private void AutoHoover() {
        Vector2 vel = playerRb.velocity;
        if (!Mathf.Approximately(vel.y, 0))
        {
            float y = 0.25f * Mathf.Abs(vel.y);
            y = Mathf.Clamp(y, 0.15f, 1);
            currentPower -= Mathf.Sign(vel.y) * (y);
        }
    }

    private void Thrust() {
        playerRb.AddForce(playerRb.transform.up * (currentPower * powerMultiplier) * Time.deltaTime);
    }

    public void Idle() {
        AutoHoover();
    }

    private void HandlePower(Touch touch) {
        if (currentPower <= maxPower && currentPower >= minPower)
        {
            if (touch.deltaTime != 0f) currentPower += (((touch.deltaPosition.y / Screen.height) * (maxPower - minPower)) * powerSensitivity) * (Time.deltaTime / touch.deltaTime);
            else currentPower += (((touch.deltaPosition.y / Screen.height) * (maxPower - minPower)) * powerSensitivity);
        }
        if (currentPower < minPower) currentPower = minPower;
        if (currentPower > maxPower) currentPower = maxPower;        
    }

    private void HandleRotation(Touch touch) {
        //Rotation
        if ((currentAngle < maxTiltSpeed || touch.deltaPosition.x < 0f) ||
            (currentAngle > 360f - maxTiltSpeed || touch.deltaPosition.x > 0f)) currentAngle -= touch.deltaPosition.x * rotationSensitivity;

        if (currentAngle < 0f) currentAngle += 360f;
        else if (currentAngle > 360f) currentAngle -= 360f;

        if (currentAngle > maxTiltSpeed && currentAngle < 180f) currentAngle = maxTiltSpeed;
        else if (currentAngle < 360f - maxTiltSpeed && currentAngle > 180f) currentAngle = 360f - maxTiltSpeed;

        //Flip the copter depending on its angle
        if (currentAngle > 180f) playerCopter.Direction(true);
        else if (currentAngle < 180f) playerCopter.Direction(false);
    }
}
