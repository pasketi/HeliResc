using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TutorialScript : MonoBehaviour {

    private bool useFuel, useRepair, useFinish; //Trigger 1 fuel, trigger 2 repair, trigger 3 goals
    private bool tr1Enter, tr2Enter;

    private TutorialPelican pelican;
    private bool pelicanCollided;
    public bool FuelRepair { get { return (useFuel && useRepair); } }

    public Image arrowImage;

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

    private void HitPelicanTrigger() {
        playerRB.isKinematic = true;
    }
    private void HitPelican(string tag) {
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
            }
        }

        switch (name) {
            case "LandingBoat2":
                TriggerEnter(1);
                landing.ShowFuel(true);
                break;
            case "PalmIsland east":
                TriggerEnter(2);
                landing.ShowRepair(true);
                break;
            case "LandingBoat":
                landing.ShowVictory(true);
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

        playerRB.isKinematic = false;
    }

    private void LockPlayer(bool lockPlayer) {
        
    }

    #region Step_Methods
    private IEnumerator Step1() {
        FingerAnimation finger = GameObject.FindObjectOfType<FingerAnimation>();

        arrowImage.enabled = false;

        playerRB.isKinematic = true;

        while (!finger.finished) {
            yield return null;
        }

        playerRB.isKinematic = false;

        arrowImage.enabled = true;

        StartCoroutine(Step2());
        StartCoroutine(Step7());
    }
    private IEnumerator Step2() {

        while (!tr1Enter) {
            yield return null;
        }
        StartCoroutine(Step3());
    }
    private IEnumerator Step3() {
        playerRB.isKinematic = true;
        while (!useFuel)
        {
            yield return null;
        }
        playerRB.isKinematic = false;
        StartCoroutine(Step4());
    }
    private IEnumerator Step4() {

        while (!pelicanCollided)
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
    }
    private IEnumerator Step7() {
        while (!useFinish) {
            yield return null;
        }        
    }
    #endregion
}
