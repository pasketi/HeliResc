using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FingerAnimation : MonoBehaviour {

    public Button okButton;     //Button to close the first tutorial screen

    private Animator animator;

    private bool showVertical;
    private Copter player;

    [HideInInspector]
    public bool finished;

    private bool clickedOnce;

    void Awake() {
        okButton.interactable = false;        
    }

    // Use this for initialization
    void Start () {
        
        finished = false;
        clickedOnce = false;

        
        animator = GetComponent<Animator>();

        animator.Play("FirstTip");

        Invoke("ActivateButton", 1.25f);
        player = GameObject.Find("Copter").GetComponent<Copter>();
        player.Kinematic(true);
    }

    private void ActivateButton() {
        okButton.interactable = true;
    }

    public void ClickedOK() {                    
        if(clickedOnce == true)
        {
            finished = true;
            player.Kinematic(false);
            gameObject.SetActive(false);
        } else {
            clickedOnce = true;
            animator.Play("SecondTip");
        }
    }
}
