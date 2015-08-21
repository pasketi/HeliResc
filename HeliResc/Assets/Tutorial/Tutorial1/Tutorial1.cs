using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial1 : TutorialScript {
	
    public Text text;
    public GameObject holdTipImage;
    
    private bool clickedOnce;
    private LevelManager manager;
    

    // Use this for initialization
    protected override void Start () {
		base.Start ();
        clickedOnce = false;

        manager = GameObject.FindObjectOfType<LevelManager>();
    }

    void Update() {
        if (manager.gameState == GameState.Running) {
            if (Input.touchCount > 0 || Input.GetMouseButton(0)) {
                holdTipImage.SetActive(false);
            }
            else
                holdTipImage.SetActive(true);
        }
        else
            holdTipImage.SetActive(false);
    }
	
    public override void ClickedOK() {                    
        if(clickedOnce == true)
        {
            GameObject.Find("Copter").GetComponent<Copter>().Kinematic(false);

            animator.enabled = false;
            GetComponent<Image>().enabled = false;

            for (int i = transform.childCount-1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            manager.StartGame();
        } else {
            clickedOnce = true;
            text.text = "Tilt";            
            animator.Play("SecondTip");
        }
    }
}

