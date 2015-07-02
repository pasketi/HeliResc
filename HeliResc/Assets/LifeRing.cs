using UnityEngine;
using System.Collections;

public class LifeRing : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("ActionableObject")) {
            ActionableObject ao = other.GetComponent<ActionableObject>();
            ao.UseAction();
            gameObject.SetActive(false);
        }
    }
}
