﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TutorialScript : MonoBehaviour {

    private bool getInput = false;

    private bool useFuel, useRepair, useFinish; //Trigger 1 fuel, trigger 2 repair, trigger 3 goals
    private bool tr1Enter, tr2Enter;

    private TutorialPelican pelican;
    private bool pelicanCollided;
    public bool FuelRepair { get { return (useFuel && useRepair); } }


    private Rigidbody2D playerRB;

    private LandingPadManager[] landingPads;

    //private string[] landingName = { "PalmIsland west", "LandingBoat2", "PalmIsland east", "LandingBoat" };

    void OnEnable() {
        EventManager.StartListening("Fuel", PressFuel);
        EventManager.StartListening("Repair", PressRepair);
        EventManager.StartListening("Finish", PressFinish);
    }
    void OnDisable() {
        EventManager.StopListening("Fuel", PressFuel);
        EventManager.StopListening("Repair", PressRepair);
        EventManager.StopListening("Finish", PressFinish);
    }

	// Use this for initialization
	void Start () {

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

        StartCoroutine(Step1());
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
        pelicanCollided = true;
        playerRB.isKinematic = false;
    }

    private void PressFuel() { useFuel = true; }
    private void PressRepair() { useRepair = true; }
    private void PressFinish() { useFinish = true; }

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
                break;
            case "PalmIsland east":
                landing.HideRepair();
                break;
            case "LandingBoat":
                landing.HideVictory();
                break;
        }
    }

    private void LockPlayer(bool lockPlayer) {
        
    }

    #region Step_Methods
    private IEnumerator Step1() {
        FingerAnimation finger = GameObject.FindObjectOfType<FingerAnimation>();
        Debug.Log("Step1 start");

        getInput = false;
        while (!finger.finished) {
            yield return null;
        }
        Debug.Log("Step1 end");
        StartCoroutine(Step2());
        StartCoroutine(Step7());
    }
    private IEnumerator Step2() {
        getInput = true;

        while (!tr1Enter) {
            yield return null;
        }
        Debug.Log("Step2 end");
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
        Debug.Log("Step3 end");
        StartCoroutine(Step4());
    }
    private IEnumerator Step4() {

        while (!pelicanCollided)
        {
            yield return null;
        }
        Debug.Log("Step4 end");
        StartCoroutine(Step5());
    }
    private IEnumerator Step5() {

        while (!tr2Enter)
        {
            yield return null;
        }
        Debug.Log("Step5 end");
        StartCoroutine(Step6());
    }
    private IEnumerator Step6() {

        while (!useRepair)
        {
            yield return null;
        }
        Debug.Log("Step6 end");
    }
    private IEnumerator Step7() {
        Debug.Log("Step7 start");
        while (!useFinish) {
            yield return null;
        }        
    }
    #endregion
}
