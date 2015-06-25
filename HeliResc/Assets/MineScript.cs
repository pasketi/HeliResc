using UnityEngine;
using System.Collections;

public class MineScript : MonoBehaviour {
    
    public GameObject explosion;    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CrushBall"))
        {            
            Explode();
        }
    }

    private void Explode()
    {        
        GetComponent<SpriteRenderer>().enabled = false;
        foreach (Collider2D c in GetComponents<Collider2D>())
            c.enabled = false;
        ParticleSystem[] parts = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in parts)
        {
            p.Play();
        }
        explosion.SetActive(true);
    }
}
