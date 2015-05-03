using UnityEngine;
using System.Collections;

public class TutorialTrigger : MonoBehaviour {

    public int index;
    public bool triggered;
    private TutorialScript tutorial;

	// Use this for initialization
	void Start () {
        tutorial = GameObject.FindObjectOfType<TutorialScript>();
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Player")) {
            triggered = true;
        }
    }
}
