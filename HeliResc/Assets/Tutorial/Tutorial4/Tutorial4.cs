using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial4 : MonoBehaviour {

    public Button okButton;     //Button to close the first tutorial screen

    private Animator animator;

	// Use this for initialization
	void Start () {
        okButton.interactable = false;
        Invoke("ActivateButton", 1);
        GameObject.Find("Copter").GetComponent<Copter>().Kinematic(true);
	}
    private void ActivateButton()
    {
        okButton.interactable = true;
    }

    public void ClickedOK()
    {
        GameObject.Find("Copter").GetComponent<Copter>().Kinematic(false);
        gameObject.SetActive(false);
    }
}
