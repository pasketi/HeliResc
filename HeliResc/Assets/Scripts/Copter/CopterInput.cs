using UnityEngine;
using System;
using System.Collections;

public class CopterInput {

    private Copter copter;

    private bool handleInput = true;
    private bool noInput = false;

    public Action<Touch> InputUpdate = (Touch) => { };
    public Action IdleUpdate = () => { };

    public void UpdateMethod() {
        noInput = false;
        if(handleInput == true) {
            foreach (Touch t in Input.touches) {
                noInput = true;
                InputUpdate(t);
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
