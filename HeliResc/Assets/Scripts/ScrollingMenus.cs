using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The script will move a panel back and forth in world space that holds all the menus.
/// </summary>
public class ScrollingMenus : MonoBehaviour {
	
	public GameObject[] go;		//Order of canvases: 1. MainMenu 2.LevelEnd 3. Upgrades
	public List<GameObject> menus;
	
	private float[] canvasAnchorPoints;
	private Vector3[] panelAnchorPoints;
	
	public float scrollingSpeed = 10f;		//How fast the menus will scroll from one to another
	public float slowedSpeed;
	private float originalSpeed;
	public bool showLevelEnd = false;

	private bool isScrolling;		//Is the menu moving to another view
	private bool isDragging;		//Is the player dragging the screen
	private Vector3 target;			//target point in world space
	private Transform menuPanel;	//parent of the menu gameobjects
	
	private int current = 0;			//The index of the current menu
	private Vector3 mouseStart;			//Position in screen space where player tapped the screen
	private Vector3 mouseEnd;			//Position where player's tap ended
	private float swipeAmount;			//Amount in percentage of the screen width how much the player should drag before changing the menu
	private Vector3 previousPosition;	//The mouse position in screen space where it was during previous frame
	
	// Use this for initialization
	void Start () {
		menus = new List<GameObject> ();
		swipeAmount = Screen.width * .2f;
		menuPanel = go[0].transform.parent;
		Vector3[] vecs = new Vector3[4];
		GetComponent<RectTransform>().GetWorldCorners(vecs);

		GameManager g = GameObject.Find("GameManager").GetComponent<GameManager>();
		showLevelEnd = g.ShowLevelEnd;
		current = g.CurrentMenu;
		
		for(int i = 0; i < go.Length; i++) {
			if(go[i].name.Equals("LevelEnd")) {
				go[i].SetActive(showLevelEnd);
				if(showLevelEnd)
					menus.Add(go[i]);
			}
			else
				menus.Add(go[i]);
		}
		
		canvasAnchorPoints = new float[menus.Count];
		panelAnchorPoints = new Vector3[menus.Count];
		
		for(int i = 0; i < menus.Count; i++){
			canvasAnchorPoints[i] = menus[0].transform.position.x + (i * vecs[3].x);
			panelAnchorPoints[i] = new Vector3(canvasAnchorPoints[i] - i * 4 * canvasAnchorPoints[0], menus[0].transform.position.y);
			menus[i].transform.position = new Vector3(canvasAnchorPoints[i], menus[i].transform.position.y);
		}

		current = Mathf.Clamp(current, 0, menus.Count - 1);
		target = panelAnchorPoints[current];
		menuPanel.position = target;
		
		scrollingSpeed = Screen.width * 1.25f;
		originalSpeed = scrollingSpeed;
		slowedSpeed = scrollingSpeed * 0.75f;
	}
	
	void Update() {
		if(Input.GetMouseButtonDown(0)){
			mouseStart = previousPosition = Input.mousePosition;
			isDragging = true;
		}
		if(Input.GetMouseButtonUp(0)) {
			mouseEnd = Input.mousePosition;
			if(mouseEnd.x < mouseStart.x) {			//Player dragged from right to left
				if(mouseStart.x - mouseEnd.x > swipeAmount)
					current = current < menus.Count - 1 ? current + 1 : menus.Count - 1;
			} else if(mouseEnd.x > mouseStart.x){	//Player dragged from left to right
				if(mouseEnd.x - mouseStart.x > swipeAmount)
					current = current > 0 ? current - 1 : 0;
			}
			isScrolling = true;
			isDragging = false;
			target = panelAnchorPoints[current];
		}
		if(Input.GetMouseButton(0) && isDragging) {	//Move the panel when the player is dragging
			menuPanel.position -= Vector3.right * ((Mathf.Abs(previousPosition.x) - Mathf.Abs(Input.mousePosition.x)));
			previousPosition = Input.mousePosition;
		}
		
		
		if(isScrolling) {
			float step = Time.deltaTime * scrollingSpeed;
			menuPanel.position = Vector3.MoveTowards(menuPanel.position, target, step);
			
			if(scrollingSpeed > slowedSpeed) {
				scrollingSpeed *= 0.995f;
			}
			else
				scrollingSpeed = slowedSpeed;
			
			if(Vector3.Distance(menuPanel.position, target) < 10) {		//if the panel is close enough, stop scrolling
				isScrolling = false;
				scrollingSpeed = originalSpeed;
				Debug.Log ("Reach target");
				menuPanel.position = target;
			}
		}
	}
	
	public void ShowUpgrades() {
		for (int i = 0; i < menus.Count; i++) {
			if(menus[i].name.Equals("Upgrades")) {
				current = i;
				target = panelAnchorPoints[current];
				isScrolling = true;
				isDragging = false;
			}
		}
	}
}