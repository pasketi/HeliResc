using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FingerAnimation : MonoBehaviour {



    private Animator animator;

    private bool showVertical;

    [HideInInspector]
    public bool finished;

    private bool clickedOnce;

    // Use this for initialization
    void Start () {
        
        finished = false;
        clickedOnce = false;

        animator = GetComponent<Animator>();

        animator.Play("FirstTip");
    }

    void Update() {        
       
    }

    public void ClickedOK() {                    
        if(clickedOnce == true)
        {
            finished = true;            
            gameObject.SetActive(false);
        } else {
            clickedOnce = true;
            animator.Play("SecondTip");
        }
    }
}
