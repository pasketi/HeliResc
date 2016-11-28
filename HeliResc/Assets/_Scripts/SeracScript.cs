using UnityEngine;
using System.Collections;

public class SeracScript : ActionableObject {
    
    public float requiredForce = 3;
    public GameObject explosion;
    

	// Use this for initialization
	void Start () {
            
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Hook") || other.CompareTag("CrushBall")) {
            if(other.GetComponent<Rigidbody2D>().velocity.magnitude >= requiredForce)
                UseAction();
        }
    }

    public override void UseAction() {
        GameObject.FindObjectOfType<LevelManager>().saveCrates(1);
        Explode();
    }

    private void Explode() {        
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
