using UnityEngine;
using System;
using System.Collections;

public class CopterInput {

    private Copter copter;

    private bool handleInput = true;
    private bool noInput = false;

    public Action<Touch> InputUpdate = (Touch) => { };
    public Action<Touch> TouchStart = (Touch) => { };
    public Action<Touch> TouchEnd = (Touch) => { };
    public Action IdleUpdate = () => { };

    public void UpdateMethod() {
        noInput = false;
        if(handleInput == true) {
            foreach (Touch t in Input.touches) {
                noInput = true;
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
        if (noInput == false)
            IdleUpdate();
    }
    public void DisableInput() {
        handleInput = false;
    }
    public void EnableInput() {
        handleInput = true;
    }
}
