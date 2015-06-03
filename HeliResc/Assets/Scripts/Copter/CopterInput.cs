using UnityEngine;
using System;
using System.Collections;

public class CopterInput {

    private Copter copter;

    private bool handleInput = true;
    private bool noInput = true;

    private Vector3 previousMouse;
    private float previousTime;

    public Action<MouseTouch> InputUpdate = (MouseTouch) => { };
    public Action<MouseTouch> TouchStart = (MouseTouch) => { };
    public Action<MouseTouch> TouchEnd = (MouseTouch) => { };
    public Action IdleUpdate = () => { };

#if UNITY_EDITOR
    public void UpdateMethod() {
        noInput = true;
        if(handleInput == true && Input.GetMouseButton(0)) {
            
            //If the mouse was just clicked assign current values to the variables
            if (Input.GetMouseButtonDown(0)) {
                previousMouse = Input.mousePosition;
                previousTime = Time.time;
            }

            MouseTouch t = MouseTouch.TransformMouse(Input.mousePosition, previousMouse, previousTime);
            
            previousTime = Time.time;               //Set the values ready for next frame
            previousMouse = Input.mousePosition;    //Set the value ready for next frame

            noInput = false;
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
/// Class to allow mouse input in editor
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

    public static MouseTouch TransformMouse(Vector3 currentPos, Vector3 previousPos, float previousTime) {

        MouseTouch m = new MouseTouch();

        if (Input.GetMouseButton(0)) {
            if (Input.GetMouseButtonDown(0))
                m.phase = TouchPhase.Began;
            else if (Input.GetMouseButtonUp(0))
                m.phase = TouchPhase.Ended;
            else
                m.phase = TouchPhase.Moved;
        }
        m.position = currentPos;
        m.deltaPosition = currentPos - previousPos;
        m.deltaTime = Time.time - previousTime;
        
        return m;
    }
}
