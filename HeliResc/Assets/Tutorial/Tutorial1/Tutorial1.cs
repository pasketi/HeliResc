using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial1 : TutorialScript {
	
    public Text text;

    private bool clickedOnce;
    

    // Use this for initialization
    protected override void Start () {
		base.Start ();
        clickedOnce = false;
		        
    }
	
    public override void ClickedOK() {                    
        if(clickedOnce == true)
        {
			base.ClickedOK();
        } else {
            clickedOnce = true;
            text.text = "Tilt";            
            animator.Play("SecondTip");
        }
    }
}

