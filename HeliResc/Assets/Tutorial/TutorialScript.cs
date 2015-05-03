using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TutorialScript : MonoBehaviour {

    private bool getInput = false;

    private bool useFuel, useRepair, useFinish; //Trigger 1 fuel, trigger 2 repair, trigger 3 goals
    private bool tr1Enter, tr2Enter, tr3Enter;

    private TutorialPelican pelican;
    private bool pelicanActive;
    public bool FuelRepair { get { return (useFuel && useRepair); } }

    private ObjectiveTrigger trigger;

    private Rigidbody2D playerRB;

    private LandingPadManager[] landingPads;

    private string[] landingName = { "PalmIsland west", "LandingBoat2", "PalmIsland east", "LandingBoat" };

	// Use this for initialization
	void Start () {
        trigger = GameObject.FindObjectOfType<ObjectiveTrigger>();

        pelican = GameObject.FindObjectOfType<TutorialPelican>();
        pelican.PelicanTriggered += HitPelicanTrigger;
        pelican.obstacle.ObstacleHit += HitPelican;

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

    private void HitPelicanTrigger() {
        Debug.Log("Pelican triggered");
        playerRB.isKinematic = true;
    }
    private void HitPelican(string tag) {
        Debug.Log("Pelican hit");
        playerRB.isKinematic = false;
    }

    public void TriggerExit(int trigger) {
        switch (trigger) {
            case 1:
                useFuel = true;
                break;
            case 2:
                useRepair = true;
                break;
            case 3:
                useFinish = true;
                break;
        }
    }

    public void TriggerEnter(int trigger)
    {
        switch (trigger)
        {
            case 1:
                tr1Enter = true;
                break;
            case 2:
                tr2Enter = true;
                break;
            case 3:
                tr3Enter = true;
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
                TriggerEnter(1);
                landing.ShowFuel();
                break;
            case "PalmIsland east":
                TriggerEnter(2);
                landing.ShowRepair();
                break;
            case "LandingBoat":
                TriggerEnter(3);
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
                TriggerExit(1);
                break;
            case "PalmIsland east":
                landing.HideRepair();
                TriggerExit(2);
                break;
            case "LandingBoat":
                landing.HideVictory();
                TriggerExit(3);
                break;
        }
    }

    private void LockPlayer(bool lockPlayer) {
        
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

        while (!tr1Enter) {
            yield return null;
        }        
        
        StartCoroutine(Step3());
    }
    private IEnumerator Step3() {
        getInput = false;
        playerRB.isKinematic = true;
        while (!useFuel)
        {
            yield return null;
        }
        playerRB.isKinematic = false;
        StartCoroutine(Step4());
    }
    private IEnumerator Step4() {

        while (!pelicanActive)
        {
            yield return null;
        }
        StartCoroutine(Step5());
    }
    private IEnumerator Step5() {

        while (!tr2Enter)
        {
            yield return null;
        }

        StartCoroutine(Step6());
    }
    private IEnumerator Step6() {

        while (!useRepair)
        {
            yield return null;
        }
        StartCoroutine(Step7());
    }
    private IEnumerator Step7() {
        while (!useFinish) {
            yield return null;
        }
    }
    #endregion
}
