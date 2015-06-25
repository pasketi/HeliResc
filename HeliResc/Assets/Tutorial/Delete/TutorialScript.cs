using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TutorialScript : MonoBehaviour {

    private bool useFuel; //Trigger 1 fuel, trigger 2 repair, trigger 3 goals
    private bool tr1Enter, tr2Enter;
    

    public GameObject balls1;
    public GameObject balls2;
    public GameObject balls3;

    public GameObject[] arrows;

    public GameObject fingerPointer; //References to the gameobjects of fingers pointing the buttons
    private bool onLandingPad;    
  
    public bool FuelUsed { get { return (useFuel); } }

    private Rigidbody2D playerRB;

    private PlatformButtons currentPlatformButtons;

    private LandingPadManager[] landingPads;

    //private string[] landingName = { "PalmIsland west", "LandingBoat2", "PalmIsland east", "LandingBoat" };

    void OnEnable() {
        EventManager.StartListening("Fuel", PressFuel);
    }
    void OnDisable() {
        EventManager.StopListening("Fuel", PressFuel);
    }

	// Use this for initialization
	void Start () {

        
        playerRB = GameObject.Find("Copter").GetComponent<Rigidbody2D>();

        landingPads = GameObject.FindObjectsOfType<LandingPadManager>();

        foreach (LandingPadManager l in landingPads) {
            l.ResetEvents();
            l.enterPlatform += EnterPlatform;
            l.exitPlatform += ExitPlatform;
        }


        fingerPointer.GetComponentInChildren<Animator>().Play("FingerAnimation");
        fingerPointer.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0);        

        StartCoroutine(Step1());        
	}
    
    private void PressFuel() { 
        useFuel = true; 
        currentPlatformButtons.HideFuel();
        StartCoroutine(FadeOutBalls(fingerPointer, 0.2f));
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

    private void EnterPlatform(GameObject name) {
        onLandingPad = true;
        currentPlatformButtons = null;
        foreach (LandingPadManager l in landingPads) {            
            if (l.transform.root.name.Equals(name.name)) {                
                currentPlatformButtons = l.gameObject.GetComponentInChildren<PlatformButtons>();
            }
        }

        switch (name.name) {
            case "LandingBoat2":
                TriggerEnter(1);
                currentPlatformButtons.ShowFuel(false);
                StartCoroutine(SetFingerPosition(0));
                break;            
            case "LandingBoat":
                //Winning code here: launch fireworks
                break;
        }

    }
    private void ExitPlatform(GameObject name) {
        onLandingPad = false;
        PlatformButtons landing = null;
        foreach (LandingPadManager l in landingPads)
        {
            if (l.transform.root.name.Equals(name))
            {                
                landing = l.gameObject.GetComponentInChildren<PlatformButtons>();
            }
        }

        switch (name.name)
        {
            case "LandingBoat2":
                landing.HideFuel();
                if(useFuel == false)
                    StartCoroutine(FadeOutBalls(fingerPointer, 0.2f));
                break;            
        }
        
        playerRB.isKinematic = false;
    }

    private IEnumerator SetFingerPosition(int index) {
        yield return new WaitForSeconds(0.6f);

        if (onLandingPad == false) yield break;                 //Don't show the finger if not on landing pad or...
        else if (index == 0 && useFuel == true) yield break;    //if already pressed fuel button and on the first landing pad or...
        

        StartCoroutine(FadeInBalls(fingerPointer, 0.05f));        
        Transform t = null;
        if(index == 0) {                            
            t = currentPlatformButtons.transform.Find("Fill Tank Button").Find("Position");                
        }
        Vector3 vec = new Vector3(1, 1, 0).normalized;
        Debug.Log(t.position + " Position of the rect transform. Name: " + t.name);
        fingerPointer.transform.position = t.position + (vec * 3);
    }

    #region Step_Methods
    private IEnumerator Step1() {
        Tutorial1 finger = GameObject.FindObjectOfType<Tutorial1>();

        playerRB.isKinematic = true;

        //while (!finger.finished) {
           yield return null;
        //}

        playerRB.isKinematic = false;        

        StartCoroutine(Step2());
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
        Debug.Log("Start time: " + Time.time);
        while (sprites[0].color.a < 1)
        {
            foreach (SpriteRenderer s in sprites)
            {
                Color c = s.color;
                c.a += time;
                s.color = c;                
            }
            yield return null;
        }
        Debug.Log("End time: " + Time.time);
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
            }
            yield return null;
        }
    }
}
