﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class Engine : Upgradable {

    public float powerSensitivity;
    public float rotationSensitivity;

    public float powerMultiplier;

    private FuelTank tank;

    //rotation related variables
    public float currentAngle;
    public float copterAngle = 0;
    public float maxTiltSpeed = 75;
    public float maxTiltValue = 100;
    public float returnSpeed = 5;  //Returns the copter back to 0 angle

    public float holdTime = 0.25f;  //Time to limit the return speed
    private float tempHoldTime = 0; //holder to calculate the time passed since player let go of the screen
    private float persistence = 1;  //Limits the return speed
    

    //power related variables
    public float minPower;
    public float currentPower;
    public float maxPower;    

    public void Init(Copter copter) {
        base.Init(copter);
        tank = copter.fuelTank;
        tempHoldTime = holdTime;
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        copterAngle = playerRb.transform.eulerAngles.z;        

        //Flip the copter depending on its angle
        if (playerRb.velocity.x > 0)
        {
            playerCopter.Direction(true);   //True means to turn right
        }
        else if (playerRb.velocity.x < 0)
        {
            playerCopter.Direction(false);
        }
        Thrust();
    }

    public override void TouchStart(Touch touch) {
        currentAngle = copterAngle;
    }
    public override void TouchEnd(Touch touch)
    {
        playerCopter.transform.localScale = new Vector3(Random.Range(0.5f,3), Random.Range(0.5f,3));
        tempHoldTime = 0;
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
        IdleRotation();
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

        if ((currentAngle < maxTiltValue
            || touch.deltaPosition.x < 0f)
            || (currentAngle > 360f - maxTiltValue
            || touch.deltaPosition.x > 0f))
        {
            currentAngle -= touch.deltaPosition.x * rotationSensitivity;
        }

        if (currentAngle < 0f)
        {
            currentAngle += 360f;
        }
        else if (currentAngle > 360f)
        {
            currentAngle -= 360f;
        }

        if (currentAngle > maxTiltValue && currentAngle < 180f)
        {
            currentAngle = maxTiltValue;
        }
        else if (currentAngle < 360f - maxTiltValue && currentAngle > 180f)
        {
            currentAngle = 360f - maxTiltValue;
        }        
        
        if (copterAngle != currentAngle)
        { // Turn to currentAngle
            if (currentAngle < 180f)
            {
                if (copterAngle > 180f)
                {
                    // Rotate CCW
                    playerRb.transform.Rotate(new Vector3(0f, 0f, maxTiltSpeed * Time.deltaTime));
                }
                else if (copterAngle < 180f)
                {
                    if (copterAngle < currentAngle)
                    {
                        // Rotate CCW
                        playerRb.transform.Rotate(new Vector3(0f, 0f, maxTiltSpeed * Time.deltaTime));
                    }
                    else if (copterAngle > currentAngle)
                    {
                        // Rotate CW
                        playerRb.transform.Rotate(new Vector3(0f, 0f, -maxTiltSpeed * Time.deltaTime));
                    }
                }
            }
            else if (currentAngle > 180f)
            {
                if (copterAngle < 180f)
                {
                    // Rotate CW
                    playerRb.transform.Rotate(new Vector3(0f, 0f, -maxTiltSpeed * Time.deltaTime));
                }
                else if (copterAngle > 180f)
                {
                    if (copterAngle < currentAngle)
                    {
                        // Rotate CCW
                        playerRb.transform.Rotate(new Vector3(0f, 0f, maxTiltSpeed * Time.deltaTime));
                    }
                    else if (copterAngle > currentAngle)
                    {
                        // Rotate CW
                        playerRb.transform.Rotate(new Vector3(0f, 0f, -maxTiltSpeed * Time.deltaTime));
                    }
                }
            }
        }
        Debug.Log("Copter Angle: " + copterAngle);
        Debug.Log("Current Angle: " + currentAngle);
    }

    private void IdleRotation() {
        if (copterAngle != 0f)
        { // Return to 0°
            if (copterAngle > 180f)
            {
                playerRb.transform.Rotate(new Vector3(0f, 0f, returnSpeed * Time.deltaTime * (360f - copterAngle) * persistence));
            }
            else if (copterAngle < 180f)
            {
                playerRb.transform.Rotate(new Vector3(0f, 0f, -(returnSpeed * Time.deltaTime) * copterAngle * persistence));
            }
        }
        if (tempHoldTime != holdTime)
        {
            tempHoldTime += Time.deltaTime;
            persistence = tempHoldTime / holdTime;
        }
        if (tempHoldTime > holdTime)
        {
            persistence = 1f;
            tempHoldTime = holdTime;
        }
    }
}