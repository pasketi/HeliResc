using UnityEngine;
using System.Collections;

public class MineScript : MonoBehaviour, SoundObject {
    
    public GameObject explosion;

    private AudioSource _audio;

    void Start() {
        _audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("CrushBall") || other.name.Equals("Copter") || other.CompareTag("Hook"))
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
        GetComponent<SpriteRenderer>().enabled = false;
        foreach (Collider2D c in GetComponents<Collider2D>())
            c.enabled = false;
        ParticleSystem[] parts = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in parts) {
            p.Play();
        }
        PlaySound();
        explosion.SetActive(true);
    }

    public void Mute(bool mute)
    {
        if (_audio == null)
            _audio = GetComponent<AudioSource>();
        _audio.volume = mute ? 1 : 0;
    }

    public void PlaySound()
    {
        _audio.Play();
    }
}
