using UnityEngine;
using System.Collections;

public class MineScript : MonoBehaviour {
    
    public GameObject explosion;    

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.name.Equals("Copter") || other.CompareTag("Hook"))
        {   
			StartCoroutine(DetonateCopter());
            Explode();
        }
    }

    private IEnumerator DetonateCopter() {
        Transform tr = GameObject.Find("Copter").transform;
        float delay = Vector3.Distance(tr.position, transform.position);    //Calculate the distance between the center of the explosion and copter
        delay = Mathf.Clamp(delay, 0, 5);                                   //Maximum distance should be 5
        delay = (2f/3) * (delay/5);                                         //Multiply the time of the animation with the percentage of the distance
        yield return new WaitForSeconds(delay);                             //Wait for the delay time
        tr.GetComponent<Copter>().Detonate();                               //Detonate the copter
    }

    private void Explode()
    {
        if (explosion == null) return;
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
