using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial1 : MonoBehaviour {

    public Button okButton;     //Button to close the first tutorial screen
    public Text text;
    private Animator animator;

    private bool clickedOnce;

    void Awake() {
        okButton.interactable = false;
    }

    // Use this for initialization
    void Start () {
        
        clickedOnce = false;

        animator = GetComponent<Animator>();

        animator.Play("FirstTip");

        Invoke("ActivateButton", 1.25f);
        GameObject.Find("Copter").GetComponent<Copter>().Kinematic(true);
    }

    private void ActivateButton() {
        okButton.interactable = true;
    }

    public void ClickedOK() {                    
        if(clickedOnce == true)
        {
            GameObject.Find("Copter").GetComponent<Copter>().Kinematic(false);
            gameObject.SetActive(false);
        } else {
            clickedOnce = true;
            text.text = "Tilt";            
            animator.Play("SecondTip");
        }
    }
}
