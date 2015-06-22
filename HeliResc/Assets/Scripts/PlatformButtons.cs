using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlatformButtons: MonoBehaviour {

    public Animator fuelAnimator;
    //public Animator victoryAnimator;
    //public Animator repairAnimator;
	
	public float buttonSize;
	
	private LandingPadManager landing;
    private LevelManager levelManager;

	private RectTransform[] rects;   

	private Transform target;

    void OnEnable() {
        EventManager.StartListening("EnterPlatform", ShowFuel);
        EventManager.StartListening("ExitPlatform", HideFuel);
    }
    void OnDisable() {
        EventManager.StopListening("EnterPlatform", ShowFuel);
        EventManager.StopListening("ExitPlatform", HideFuel);
    }

	// Use this for initialization
	void Awake () {

        fuelAnimator = GetComponentInChildren<Animator>();

		RectTransform t = GetComponent<RectTransform> ();        

		t.sizeDelta = new Vector2 (Screen.width, Screen.height) * 0.5f;

        levelManager = GameObject.FindObjectOfType<LevelManager>();

		landing = transform.parent.parent.GetComponent<LandingPadManager>();
        if (landing.Equals(null))
            Debug.LogError("The landingpad manager was not found in platform buttons");


        target = transform.parent;
        transform.position = target.position + Vector3.up * 1.5f;
    }
	
	
	/// <summary>
	/// Hides the popup window immeadiately
	/// </summary>
	public void Idle(bool fuel = true, bool repair = true, bool victory = true) {
        if (fuel == true)
            fuelAnimator.Play("ShowIdleFuel");
        //if (repair == true)
        //    repairAnimator.Play("ShowIdleRepair");
        //if (victory == true)
        //    victoryAnimator.Play("ShowIdleVictory");
	}
	
	/// <summary>
	/// Plays the disable popup window animation
	/// </summary>
	public void HideAllButtons(string name)
	{
        CancelInvoke();
        HideFuel();
        Invoke("HideRepair", 0.1f);
        Invoke("HideVictory", 0.2f);
    }
    public void StartRefill()
    {
        landing.StartRefill();
        EventManager.TriggerEvent("Fuel");
    }
    public void ShowFuel()
    {
        fuelAnimator.Play("ShowFuel");
    }
    public void ShowFuel(bool pump)
    {
        fuelAnimator.SetBool("Pump", pump);
        fuelAnimator.Play("ShowFuel");
    }
    public void HideFuel()
    {
        if (fuelAnimator == null)
            fuelAnimator = GetComponentInChildren<Animator>();
        fuelAnimator.Play("HideFuel");
    }
    ///// <summary>
    ///// Enables the popup window animation
    ///// </summary>
    //public void ShowAllButtons(string name)
    //{
    //    CancelInvoke();
    //    ShowVictory();
    //    Invoke("ShowRepair", 0.1f);
    //    Invoke("ShowFuel", 0.2f);
    //}

    //public void ShowRepair() { 
    //    repairAnimator.Play("ShowRepair");
    //}
    
    //public void ShowVictory() { 
    //    if(objectives.AnyObjectiveCompleted())
    //        victoryAnimator.Play("ShowVictory"); 
    //}

    //public void ShowRepair(bool glow)
    //{
    //    repairAnimator.SetBool("Pump", glow);
    //    repairAnimator.Play("ShowRepair");
    //}
    
    //public void ShowVictory(bool glow)
    //{
    //    if (objectives.AnyObjectiveCompleted()) {
    //        victoryAnimator.SetBool("Pump", glow);
    //        victoryAnimator.Play("ShowVictory");           
    //    }
    //}

    //public void HideRepair() { 
    //    repairAnimator.Play("HideRepair"); 
    //}
    
    //public void HideVictory() {
    //    if (objectives.AnyObjectiveCompleted())
    //        victoryAnimator.Play("HideVictory"); 
    //}
    //public void StartRepair() {
    //    landing.StartRepair();
    //    EventManager.TriggerEvent("Repair");
    //}
	    	
}
