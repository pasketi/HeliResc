using UnityEngine;
using System.Collections;

public class LifeRing : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("ActionableObject")) {
            ActionableObject ao = other.GetComponent<ActionableObject>();
            if (ao == null) { 
                Debug.LogError("actionable object component not found");
                return;
            }
            ao.UseAction();
            gameObject.SetActive(false);
        }
    }
}
