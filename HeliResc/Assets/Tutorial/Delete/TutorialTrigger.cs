using UnityEngine;
using System.Collections;

public class TutorialTrigger : MonoBehaviour {

    public int index;
    public bool triggered;	

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Copter") && !triggered) {
            triggered = true;
            Transform t = other.transform;
            t.position = new Vector3(t.position.x, transform.position.y + (other.transform.localScale.y * 0.5f));
        }
    }
}
