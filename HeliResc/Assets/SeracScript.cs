using UnityEngine;
using System.Collections;

public class SeracScript : MonoBehaviour {
    
    public float requiredForce = 3;
    public GameObject explosion;
    

	// Use this for initialization
	void Start () {
            
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("CrushBall")) {
            Debug.Log("Velocity: " + other.GetComponent<Rigidbody2D>().velocity);
            if(other.GetComponent<Rigidbody2D>().velocity.magnitude >= requiredForce)
            Explode();
        }
    }

    private void Explode() {
        GameObject.FindObjectOfType<LevelManager>().saveCrates(1);
        GetComponent<SpriteRenderer>().enabled = false;
        foreach (Collider2D c in GetComponents<Collider2D>())
            c.enabled = false;
        ParticleSystem[] parts = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in parts) {
            p.Play();
        }
        explosion.SetActive(true);
    }
}
