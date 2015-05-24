using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TutorialScript : MonoBehaviour {

    private bool useFuel, useRepair, useFinish; //Trigger 1 fuel, trigger 2 repair, trigger 3 goals
    private bool tr1Enter, tr2Enter;

    public Animator pelicanAlertAnimator;   //Reference to the animator component of the pelican alert

    public GameObject balls1;
    public GameObject balls2;
    public GameObject balls3;

    public GameObject[] fingerPointers; //References to the gameobjects of fingers pointing the buttons

    private TutorialPelican pelican;
    private bool pelicanCollided;
    public bool FuelRepair { get { return (useFuel && useRepair); } }

    private Rigidbody2D playerRB;

    private PlatformButtons currentPlatformButtons;

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

        foreach (GameObject go in fingerPointers) {
            go.GetComponentInChildren<Animator>().Play("FingerAnimation");
            go.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }

        StartCoroutine(Step1());        
	}

    private void HitPelicanTrigger() {
        playerRB.isKinematic = true;
        pelicanAlertAnimator.Play("PelicanAlert");
    }
    private void HitPelican(string tag) {
        pelicanCollided = true;
        playerRB.isKinematic = false;
    }

    private void PressFuel() { 
        useFuel = true; 
        currentPlatformButtons.HideFuel();
        StartCoroutine(FadeOutBalls(fingerPointers[0], 0.2f));
    }
    private void PressRepair() { 
        useRepair = true; 
        currentPlatformButtons.HideRepair();
        StartCoroutine(FadeOutBalls(fingerPointers[1], 0.2f));
    }
    private void PressFinish() { 
        useFinish = true; 
        currentPlatformButtons.HideVictory();
        StartCoroutine(FadeOutBalls(fingerPointers[2], 0.2f));
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
        }
    }

    private void EnterPlatform(string name) {
        currentPlatformButtons = null;
        foreach (LandingPadManager l in landingPads) {            
            if (l.transform.root.name.Equals(name)) {
                currentPlatformButtons = l.gameObject.GetComponentInChildren<PlatformButtons>();
            }
        }

        switch (name) {
            case "LandingBoat2":
                TriggerEnter(1);
                currentPlatformButtons.ShowFuel(false);
                StartCoroutine(FadeInBalls(fingerPointers[0], 0.05f));
                break;
            case "PalmIsland east":
                TriggerEnter(2);
                currentPlatformButtons.ShowRepair(false);
                StartCoroutine(FadeInBalls(fingerPointers[1], 0.05f));
                break;
            case "LandingBoat":
                currentPlatformButtons.ShowVictory(false);
                StartCoroutine(FadeInBalls(fingerPointers[2], 0.05f));
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
                StartCoroutine(FadeOutBalls(fingerPointers[0], 0.2f));
                break;
            case "PalmIsland east":
                landing.HideRepair();
                StartCoroutine(FadeOutBalls(fingerPointers[1], 0.2f));
                break;
            case "LandingBoat":
                landing.HideVictory();
                StartCoroutine(FadeOutBalls(fingerPointers[2], 0.2f));
                break;
        }

        playerRB.isKinematic = false;
    }

    private void LockPlayer(bool lockPlayer) {
        
    }

    #region Step_Methods
    private IEnumerator Step1() {
        FingerAnimation finger = GameObject.FindObjectOfType<FingerAnimation>();

        playerRB.isKinematic = true;

        while (!finger.finished) {
            yield return null;
        }

        playerRB.isKinematic = false;        

        StartCoroutine(Step2());
        StartCoroutine(Step7());
    }
    private IEnumerator Step2() {

        StartCoroutine(FadeInBalls(balls1, 0.05f));
        while (!tr1Enter) {
            yield return null;
        }
        StartCoroutine(Step3());
    }
    private IEnumerator Step3() {
        //playerRB.isKinematic = true;

        StartCoroutine(FadeOutBalls(balls1, 0.3f));
        while (!useFuel)
        {
            yield return null;
        }

        StartCoroutine(FadeInBalls(balls2, 0.05f));
        //playerRB.isKinematic = false;
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
        StartCoroutine(FadeOutBalls(balls2, 0.3f));
        while (!useRepair)
        {
            yield return null;
        }
        StartCoroutine(FadeInBalls(balls3, 0.05f));
    }
    private IEnumerator Step7() {
        StartCoroutine(FadeOutBalls(balls3, 0.3f));
        while (!useFinish) {
            yield return null;
        }        
    }
    #endregion

    private IEnumerator FadeInBalls(GameObject ballParent, float time)
    {
        SpriteRenderer[] sprites = ballParent.GetComponentsInChildren<SpriteRenderer>();
        

        foreach (SpriteRenderer s in sprites)
        {
            Color c = s.color;
            c.a = 0;
            s.color = c;
        }
        while (sprites[0].color.a < 1)
        {
            foreach (SpriteRenderer s in sprites)
            {
                Color c = s.color;
                c.a += time;
                s.color = c;
                yield return null;
            }
        }
    }
    private IEnumerator FadeOutBalls(GameObject ballParent, float time)
    {
        SpriteRenderer[] sprites = ballParent.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer s in sprites)
        {
            Color c = s.color;
            c.a = 1;
            s.color = c;
        }
        while (sprites[0].color.a > 0)
        {
            foreach (SpriteRenderer s in sprites)
            {
                Color c = s.color;
                c.a -= time;
                s.color = c;
                yield return null;
            }
        }
    }
}
