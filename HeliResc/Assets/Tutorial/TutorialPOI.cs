using UnityEngine;
using System.Collections;

public class TutorialPOI : MonoBehaviour {

    public int index;

    private bool activated = false;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Copter") && !activated) {
            activated = true;
            EventManager.TriggerEvent("NextPOI");
        }
    }
}
