using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial3 : MonoBehaviour {

    public Button okButton;     //Button to close the first tutorial screen

    private Animator animator;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        animator.Play("HookAnimation");
    }

    public void ClickedOK() {
        gameObject.SetActive(false);
    }
}
