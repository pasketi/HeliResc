using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The script will move a panel back and forth in world space that holds all the menus.
/// </summary>
public class ScrollingMenus : MonoBehaviour {
	
	public GameObject[] go;		//Order of canvases: 1. MainMenu 2.LevelEnd 3. Upgrades
	public List<GameObject> menus;
    
    public RectTransform copterSelect;
    public RectTransform moneyPanel;        //Reference to money on top right corner
    public float moneyScrollSpeed = 400;
    private float moneyHideHeight;          //Height to hide the money panel
	
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
    private int previousMenu = 0;       //Previous menus index
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
            //else if(go[i].name.Equals("Leaderboard"))
            //{
            //    if (Level.Load(LevelHandler.GetLevelSet(0).levelSetName, 0).star1)
            //    {
            //        menus.Add(go[i]);
            //    }
            //}
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

        moneyPanel.sizeDelta = new Vector2(Screen.width * 0.3f, Screen.height * 0.12f);    //Set the size of the money panel
        moneyHideHeight = Screen.height * 0.35f;
        if (menus[current].name.Equals("StartMenu") || menus[current].name.Equals("Leaderboard")) {
            moneyPanel.anchoredPosition = new Vector2(0, moneyHideHeight);
        }
                
	}
	
	void LateUpdate() {
        
        
		if(Input.GetMouseButtonDown(0)){

            Vector2 sizeDelta = new Vector2(Screen.width * (copterSelect.anchorMax.x - copterSelect.anchorMin.x), Screen.height * (copterSelect.anchorMax.y - copterSelect.anchorMin.y));
            Vector2 copterSelectPos = (Vector2)copterSelect.transform.position;
            
            copterSelectPos -= sizeDelta * 0.5f;
            //copterSelectPos.y += sizeDelta.y * 0.5f;

            Rect copterRect = new Rect(copterSelectPos.x, copterSelectPos.y, sizeDelta.x, sizeDelta.y);            

            if (!copterRect.Contains((Vector2)Input.mousePosition)) {
			    mouseStart = previousPosition = Input.mousePosition;
			    isDragging = true; 
            }
		}
		if(Input.GetMouseButtonUp(0)) {
            previousMenu = current;
			mouseEnd = Input.mousePosition;
			if(mouseEnd.x < mouseStart.x && isDragging == true) {			//Player dragged from right to left
                if (mouseStart.x - mouseEnd.x > swipeAmount) {
                    current = current < menus.Count - 1 ? current + 1 : menus.Count - 1;                    
                    StartCoroutine(SetMoneyPanelPosition());
                }
			} else if(mouseEnd.x > mouseStart.x && isDragging == true){	    //Player dragged from left to right
                if (mouseEnd.x - mouseStart.x > swipeAmount) {
                    current = current > 0 ? current - 1 : 0;
                    StartCoroutine(SetMoneyPanelPosition());
                }
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

				menuPanel.position = target;
                if (menus[current].name.Equals("Upgrades"))
                    GameManager.LoadLevel("LevelMap");
			}
		}
	}

    private IEnumerator SetMoneyPanelPosition() {
        if (menus[current].name.Equals("StartMenu") || menus[current].name.Equals("Leaderboard")) { 
            float previousTime = Time.time;
            while (moneyPanel.anchoredPosition.y < moneyHideHeight) {
                moneyPanel.anchoredPosition += Vector2.up * (Time.time-previousTime) * moneyScrollSpeed;
                previousTime = Time.time;
                yield return null;
            }
            moneyPanel.anchoredPosition = Vector2.up * moneyHideHeight;
        }
        else if (menus[previousMenu].name.Equals("StartMenu") || menus[previousMenu].name.Equals("Leaderboard")) {            
            float previousTime = Time.time;
            while (moneyPanel.anchoredPosition.y > 0) {
                moneyPanel.anchoredPosition -= Vector2.up * (Time.time - previousTime) * moneyScrollSpeed;
                previousTime = Time.time;
                yield return null;
            }
            moneyPanel.anchoredPosition = Vector2.zero;
        }
    }
	
	public void ShowUpgrades() {
        previousMenu = current;
		for (int i = 0; i < menus.Count; i++) {
			if(menus[i].name.Equals("Upgrades")) {
				current = i;
				target = panelAnchorPoints[current];
				isScrolling = true;
				isDragging = false;
			}
		}
        StartCoroutine(SetMoneyPanelPosition());
	}

    public void ShowMainMenu() {
        previousMenu = current;
        current = 1;
        target = panelAnchorPoints[current];
        isScrolling = true;
        isDragging = false;
        StartCoroutine(SetMoneyPanelPosition());
    }


    public void ShowLevelMap() {
        GameManager.LoadLevel("LevelMap");
    }
}
