using UnityEngine;
using System;
using System.Collections;

public class CopterInput {

    private Copter copter;                  //reference to copter currently in game

    private bool handleInput = true;        //Should the input be reported
    private bool noInput = true;            //Has there been any input

    private Vector3 previousMouse;          //For editor input to track deltaposition
    private float previousTime;             //For editor input to track deltatime

    public Action<MouseTouch> InputUpdate = (MouseTouch) => { };    //Event to trigger when the mouse/touch has moved
    public Action<MouseTouch> TouchStart = (MouseTouch) => { };     //Event when the mouseclick happens
    public Action<MouseTouch> TouchEnd = (MouseTouch) => { };       //Event to trigger when mouse button goes up
    public Action IdleUpdate = () => { };                           //Event when there is no input

#if UNITY_EDITOR
    public void UpdateMethod() {
        noInput = true;
        bool mouseUp = Input.GetMouseButtonUp(0);
        
        if(handleInput == true && (Input.GetMouseButton(0) || mouseUp)) {
            
            //If the mouse was just clicked assign current values to the variables
            if (Input.GetMouseButtonDown(0)) {
                previousMouse = Input.mousePosition;
                previousTime = Time.time;
            }

            MouseTouch t = MouseTouch.TransformMouse(Input.mousePosition, previousMouse, previousTime, mouseUp);
            
            previousTime = Time.time;               //Set the values ready for next frame
            previousMouse = Input.mousePosition;    //Set the value ready for next frame

            noInput = false;

            //Decide which event to trigger
            switch (t.phase) {
                case TouchPhase.Began:
                    TouchStart(t);
                    break;
                case TouchPhase.Moved:
                    InputUpdate(t);
                    break;
                case TouchPhase.Ended:
                    TouchEnd(t);
                    break;
            }            
        }
        if (noInput == true)
            IdleUpdate();
    }
#elif UNITY_ANDROID || UNITY_IPHONE
    public void UpdateMethod() {
        noInput = true;
        if(handleInput == true) {
            foreach (Touch touch in Input.touches) {
                noInput = false;
                MouseTouch t = MouseTouch.TransformTouch(touch);        //Transform the unity touch to mouse touch

                //Decide which event to trigger
                switch (t.phase) {
                    case TouchPhase.Began:
                        TouchStart(t);
                        break;
                    case TouchPhase.Moved:
                        InputUpdate(t);
                        break;
                    case TouchPhase.Ended:
                        TouchEnd(t);
                        break;
                }
            }
        }
        if (noInput == true)
            IdleUpdate();
    }
#endif


    public void DisableInput() {
        handleInput = false;
    }
    public void EnableInput() {
        handleInput = true;
    }
}

/// <summary>
/// Class to change mouse and touch input to the same type
/// </summary>
public class MouseTouch {

    public TouchPhase phase;    
    public float deltaTime;
    public Vector3 deltaPosition;
    public Vector3 position;

    public static MouseTouch TransformTouch(Touch touch) {
        MouseTouch m = new MouseTouch();
        m.phase = touch.phase;
        m.position = touch.position;
        m.deltaTime = touch.deltaTime;
        m.deltaPosition = touch.deltaPosition;

        return m;
    }

    public static MouseTouch TransformMouse(Vector3 currentPos, Vector3 previousPos, float previousTime, bool mouseUp) {

        MouseTouch m = new MouseTouch();
        
        //Set the phase for the mousetouch
        if (Input.GetMouseButton(0)) {                        
            if (Input.GetMouseButtonDown(0))
                m.phase = TouchPhase.Began;
            else
                m.phase = TouchPhase.Moved;
        } else if (mouseUp) { 
            m.phase = TouchPhase.Ended;
        }

        m.position = currentPos;
        m.deltaPosition = currentPos - previousPos;
        m.deltaTime = Time.time - previousTime;
        
        return m;
    }
}
