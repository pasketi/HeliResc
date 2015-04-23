using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlatformButtons: MonoBehaviour {

    public Animator fuelAnimator;
    public Animator victoryAnimator;
    public Animator repairAnimator;
	
	public float buttonSize;
	
	private LandingPadManager landing;
    private LevelManager levelManager;
	
	private RectTransform[] rects;
	
	private Transform target;
	
	// Use this for initialization
	void Start () {
		//popup = GetComponent<Animator>();
		//popup.Play("default");
		
		//copter = GetComponentInParent<Transform>();
		
		rects = GetComponentsInChildren<RectTransform>();
		if(rects == null)
			Debug.Log("rect array is null");
		
		//float width = Screen.width * buttonSize;
		
		RectTransform t = GetComponent<RectTransform> ();

		t.sizeDelta = new Vector2 (Screen.width, Screen.height) * 0.5f;

        levelManager = GameObject.FindObjectOfType<LevelManager>();

		landing = transform.parent.parent.GetComponent<LandingPadManager>();
        if (landing.Equals(null))
            Debug.LogError("The landingpad manager was not found in platform buttons");
		landing.enterPlatform += ShowAllButtons;
		landing.exitPlatform += HideAllButtons;

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
	public void HideAllButtons()
	{
        CancelInvoke();
        HideFuel();
        Invoke("HideRepair", 0.1f);
        Invoke("HideVictory", 0.2f);
    }
	
	/// <summary>
	/// Enables the popup window animation
	/// </summary>
	public void ShowAllButtons()
	{
        CancelInvoke();
        ShowVictory();
        Invoke("ShowRepair", 0.1f);
        Invoke("ShowFuel", 0.2f);
	}

    public void ShowRepair() { 
		Debug.Log("Animation called: Repair"); 
		repairAnimator.Play("ShowRepair"); 
	}

    public void ShowFuel() { 
		Debug.Log("Animation called: Fuel"); 
		fuelAnimator.Play("ShowFuel"); 
	}

    public void ShowVictory() { 
		Debug.Log("Animation called: Victory"); 
		victoryAnimator.Play("ShowVictory"); 
	}


    public void HideRepair() { 
		repairAnimator.Play("HideRepair"); 
	}
    public void HideFuel() { 
		fuelAnimator.Play("HideFuel"); 
	}
    public void HideVictory() { 
		victoryAnimator.Play("HideVictory"); 
	}

    public void StartRepair() {
		landing.StartRepair();
	}
	
	public void StartRefill() {
		landing.StartRefill();
	}

    public void PressFinish() {
        levelManager.pressFinishButton();
    }
	
}
