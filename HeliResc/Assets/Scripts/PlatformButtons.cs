using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlatformButtons: MonoBehaviour {

    public Animator fuelAnimator;
    //public Animator victoryAnimator;
    //public Animator repairAnimator;
	
	private LandingPadManager landing;
    private LevelManager levelManager;

	private RectTransform[] rects;   

	private Transform target;

    void OnEnable() {
        landing = transform.parent.parent.GetComponent<LandingPadManager>();
        if (landing.Equals(null))
            Debug.LogError("The landingpad manager was not found in platform buttons");

        landing.enterPlatform += ShowFuel;
        landing.exitPlatform += HideFuel;
        //EventManager.StartListening(SaveStrings.eEnterPlatform, ShowFuel);
        //EventManager.StartListening(SaveStrings.eExitPlatform, HideFuel);
    }
    void OnDisable() {

        landing.enterPlatform -= ShowFuel;
        landing.exitPlatform -= HideFuel;
        //EventManager.StopListening(SaveStrings.eEnterPlatform, ShowFuel);
        //EventManager.StopListening(SaveStrings.eExitPlatform, HideFuel);
    }

	// Use this for initialization
	void Awake () {
        landing = transform.parent.parent.GetComponent<LandingPadManager>();
        fuelAnimator = GetComponentInChildren<Animator>();		

		Vector3 landingScale = landing.transform.localScale;
		Vector3 scale = transform.localScale;

		//Set the scale of the canvas to match the platforms scale
		scale.x /= landingScale.x;
		scale.y /= landingScale.y;

		transform.localScale = scale;

		RectTransform t = GetComponent<RectTransform> ();

		Vector2 canvasSize = new Vector2 (Screen.width, Screen.height) * 0.5f;

		t.sizeDelta = canvasSize;

        levelManager = GameObject.FindObjectOfType<LevelManager>();


        target = transform.parent;
        transform.position = target.position + Vector3.up * 1.5f;
    }
	
	
	/// <summary>
	/// Hides the popup window immeadiately
	/// </summary>
	public void Idle(bool fuel = true, bool repair = true, bool victory = true) {
        if (fuel == true)
            fuelAnimator.Play("ShowIdleFuel");         
	}
	
	/// <summary>
	/// Plays the disable popup window animation
	/// </summary>
	public void HideAllButtons(string name)
	{
        CancelInvoke();
        HideFuel();
    }
    public void StartRefill()
    {
        landing.StartRefill();
    }
    public void ShowFuel(GameObject go) {
        if (levelManager.gameState.Equals(GameState.Running))
        {
            //Debug.Log("Fuel animation");
            fuelAnimator.Play("ShowFuel");
        }
    }
    public void ShowFuel()
    {
        if (levelManager.gameState.Equals(GameState.Running))
        {
            //Debug.Log("Fuel animation");
            fuelAnimator.Play("ShowFuel");
        }
    }
    public void HideFuel(GameObject go)
    {
        if (fuelAnimator == null)
            fuelAnimator = GetComponentInChildren<Animator>();
        fuelAnimator.Play("HideFuel");
    }
    public void HideFuel()
    {
        if (fuelAnimator == null)
            fuelAnimator = GetComponentInChildren<Animator>();
        fuelAnimator.Play("HideFuel");
    }	    	
}
