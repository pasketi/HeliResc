using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial1 : MonoBehaviour {

    public Button okButton;     //Button to close the first tutorial screen
    public Text text;
    private Animator animator;

    private bool showVertical;

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
        GameObject.Find("Copter").GetComponent<Copter>().Kinematic(true);
    }

    private void ActivateButton() {
        okButton.interactable = true;
    }

    public void ClickedOK() {                    
        if(clickedOnce == true)
        {
            finished = true;            
            gameObject.SetActive(false);
        } else {
            clickedOnce = true;
            text.text = "Tilt";
            GameObject.Find("Copter").GetComponent<Copter>().Kinematic(false);
            animator.Play("SecondTip");
        }
    }
}
