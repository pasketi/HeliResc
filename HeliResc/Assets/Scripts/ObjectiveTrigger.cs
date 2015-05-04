using UnityEngine;
using System.Collections;

public class ObjectiveTrigger : MonoBehaviour {

    public bool triggered;

	// Use this for initialization
	void Start () {
        triggered = false;       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Player")) {
            HitObjective();
        }
    }

    private void HitObjective() {
        triggered = true;
    }
}
