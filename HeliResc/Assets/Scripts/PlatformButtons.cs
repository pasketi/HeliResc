﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlatformButtons: MonoBehaviour {

    public Animator fuelAnimator;
    public Animator victoryAnimator;
    public Animator repairAnimator;
	
	public float buttonSize;
	
	private LandingPadManager landing;
    private LevelManager levelManager;

    private MissionObjectives objectives;
	
	private RectTransform[] rects;
	
	private Transform target;
	
	// Use this for initialization
	void Awake () {
		
		RectTransform t = GetComponent<RectTransform> ();

		t.sizeDelta = new Vector2 (Screen.width, Screen.height) * 0.5f;

        levelManager = GameObject.FindObjectOfType<LevelManager>();

		landing = transform.parent.parent.GetComponent<LandingPadManager>();
        if (landing.Equals(null))
            Debug.LogError("The landingpad manager was not found in platform buttons");
		landing.enterPlatform += ShowAllButtons;
		landing.exitPlatform += HideAllButtons;

        objectives = GameObject.Find("Objectives").GetComponent<MissionObjectives>();

        target = transform.parent;
        transform.position = target.position + Vector3.up * 1.5f;
    }
	
	
	/// <summary>
	/// Hides the popup window immeadiately
	/// </summary>
	public void Stop() {
		//popup.Play("default");
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
	
	/// <summary>
	/// Enables the popup window animation
	/// </summary>
	public void ShowAllButtons(string name)
	{
        CancelInvoke();
        ShowVictory();
        Invoke("ShowRepair", 0.1f);
        Invoke("ShowFuel", 0.2f);
	}

    public void ShowRepair() { 
		repairAnimator.Play("ShowRepair");
	}
    public void ShowFuel() { 
		fuelAnimator.Play("ShowFuel"); 
	}
    public void ShowVictory() { 
        if(objectives.AnyObjectiveCompleted())
		    victoryAnimator.Play("ShowVictory"); 
	}

    public void ShowRepair(bool glow)
    {
        repairAnimator.SetBool("Pump", glow);
        repairAnimator.Play("ShowRepair");
    }

    public void ShowFuel(bool pump)
    {
        fuelAnimator.SetBool("Pump", pump);
        fuelAnimator.Play("ShowFuel");        
    }

    public void ShowVictory(bool glow)
    {
        if (objectives.AnyObjectiveCompleted()) {
            victoryAnimator.SetBool("Pump", glow);
            victoryAnimator.Play("ShowVictory");           
        }
    }

    public void HideRepair() { 
		repairAnimator.Play("HideRepair"); 
	}
    public void HideFuel() { 
		fuelAnimator.Play("HideFuel"); 
	}
    public void HideVictory() {
        if (objectives.AnyObjectiveCompleted())
		    victoryAnimator.Play("HideVictory"); 
	}
    public void StartRepair() {
		landing.StartRepair();
        EventManager.TriggerEvent("Repair");
	}
	public void StartRefill() {
		landing.StartRefill();
        EventManager.TriggerEvent("Fuel");
    }
    public void PressFinish() {
        EventManager.TriggerEvent("Finish");
        levelManager.pressFinishButton();
    }
	
}
