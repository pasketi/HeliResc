using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TutorialScript : MonoBehaviour {

    private bool getInput = false;

    private bool trigger1, trigger2, trigger3; //Trigger 1 fuel, trigger 2 repair, trigger 3 goals
    public bool FuelRepair { get { return (trigger1 && trigger2); } }

    private ObjectiveTrigger trigger;

    private Rigidbody2D playerRB;

    private LandingPadManager[] landingPads;

    private string[] landingName = { "PalmIsland west", "LandingBoat2", "PalmIsland east", "LandingBoat" };

	// Use this for initialization
	void Start () {
        trigger = GameObject.FindObjectOfType<ObjectiveTrigger>();

        playerRB = GameObject.Find("Copter").GetComponent<Rigidbody2D>();

        landingPads = GameObject.FindObjectsOfType<LandingPadManager>();

        foreach (LandingPadManager l in landingPads) {
            l.ResetEvents();
            l.enterPlatform += EnterPlatform;
            l.exitPlatform += ExitPlatform;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (getInput) Input();
	}

    private void Input() { }

    public void TriggerEntered(int trigger) {
        switch (trigger) {
            case 1:
                trigger1 = true;
                break;
            case 2:
                trigger2 = true;
                break;
            case 3:
                trigger3 = true;
                break;
        }
    }

    private void EnterPlatform(string name) {
        PlatformButtons landing = null;
        foreach (LandingPadManager l in landingPads) {            
            if (l.transform.root.name.Equals(name)) {                
                landing = l.gameObject.GetComponentInChildren<PlatformButtons>();
                Debug.Log(landing.transform.name);
            }
        }

        switch (name) {
            case "LandingBoat2":
                landing.ShowFuel();
                break;
            case "PalmIsland east":
                landing.ShowRepair();
                break;
            case "LandingBoat":
                landing.ShowVictory();
                break;
        }

    }
    private void ExitPlatform(string name) {
        PlatformButtons landing = null;
        foreach (LandingPadManager l in landingPads)
        {
            if (l.transform.root.name.Equals(name))
            {
                landing = l.gameObject.GetComponentInChildren<PlatformButtons>();
                Debug.Log(landing.transform.name);
            }
        }

        switch (name)
        {
            case "LandingBoat2":
                landing.HideFuel();
                TriggerEntered(1);
                break;
            case "PalmIsland east":
                landing.HideRepair();
                TriggerEntered(2);
                break;
            case "LandingBoat":
                landing.HideVictory();
                TriggerEntered(3);
                break;
        }
    }

    #region Step_Methods
    private IEnumerator Step1() {
        FingerAnimation finger = GameObject.FindObjectOfType<FingerAnimation>();
        getInput = false;
        while (!finger.finished) {
            yield return null;
        }
        StartCoroutine(Step2());
    }
    private IEnumerator Step2() {
        getInput = true;
        while (!trigger1) {
            yield return null;
        }
        StartCoroutine(Step3());
    }
    private IEnumerator Step3() {
        getInput = false;
        yield return null;
        StartCoroutine(Step4());
    }
    private IEnumerator Step4() {
        yield return null;
        StartCoroutine(Step5());
    }
    private IEnumerator Step5() {
        while (!trigger2) {
            yield return null;
        }
        StartCoroutine(Step6());
    }
    private IEnumerator Step6() {
        yield return null;
        StartCoroutine(Step7());
    }
    private IEnumerator Step7() {
        while (!trigger3) {
            yield return null;
        }
    }
    #endregion
}
