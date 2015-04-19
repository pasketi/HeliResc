using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlatformButtons: MonoBehaviour {
	
	private Animator popup;         //Reference to this game objects animator
	
	public float buttonSize;
	
	private LandingPadManager landing;
	
	private RectTransform[] rects;
	
	private Transform target;
	
	// Use this for initialization
	void Start () {
		popup = GetComponent<Animator>();
		popup.Play("default");
		
		//copter = GetComponentInParent<Transform>();
		
		rects = GetComponentsInChildren<RectTransform>();
		if(rects == null)
			Debug.Log("rect array is null");
		
		//float width = Screen.width * buttonSize;
		
		RectTransform t = GetComponent<RectTransform> ();

		t.sizeDelta = new Vector2 (Screen.width, Screen.height) * 0.5f;
		
		landing = GameObject.Find("LandingBoat").GetComponentInChildren<LandingPadManager>();
		landing.enterPlatform += ShowPopup;
		landing.exitPlatform += HidePopup;

        target = transform.parent;
        transform.position = target.position + Vector3.up * 1.5f;
    }
	
	
	/// <summary>
	/// Hides the popup window immeadiately
	/// </summary>
	public void Stop() {
		popup.Play("default");
	}
	
	/// <summary>
	/// Plays the disable popup window animation
	/// </summary>
	public void HidePopup()
	{
		popup.Play("HideButtons");
	}
	
	/// <summary>
	/// Enables the popup window animation
	/// </summary>
	public void ShowPopup()
	{
		popup.Play("ShowButtons");
	}
	
	public void StartRepair() {
		landing.StartRepair();
	}
	
	public void StartRefill() {
		landing.StartRefill();
	}
	
}
