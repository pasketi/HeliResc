﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WhaleScript : MonoBehaviour {

    public Transform spawnPoint;            //The hole on the neck of the whale
    public GameObject waterDrop;
    public GameObject waterDrop2;           //Prefab water drop
    public int amountPerSecond;             //How many per second
    public float time;                      //how long to shoot bubbles
    public ParticleSystem clouds;           
    private List<GameObject> pool;          //List of all the waterDrops
    private bool isShooting;

	// Use this for initialization
	void Start () {
        clouds = GetComponentInChildren<ParticleSystem>();
        
        pool = new List<GameObject>();

        int a = (int)(time * amountPerSecond);
        for (int i = 0; i < a; i++) {
            GameObject g = null;
            if (i%3 == 0) {
                g = Instantiate(waterDrop2) as GameObject;
            } else {
                g = Instantiate(waterDrop) as GameObject;
            }
            g.transform.parent = spawnPoint;
            g.layer = LayerMask.NameToLayer("WaterDrop" + (i % 3).ToString());
            g.SetActive(false);
            pool.Add(g);
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.name.Equals("Copter")) {
			StartCoroutine(Launcher());
        }
    }

    IEnumerator Launcher() {
        if (isShooting == false) {
            isShooting = true;
            clouds.Play();
            int amount = 3;

            float startTime = Time.time;
            float remainingTime = time;
            for (int i = 0; i < pool.Count; i += amount)
            {
                for (int j = 0; j < amount; j++)
                {
                    pool[i + j].SetActive(true);
                    Rigidbody2D rb = pool[i + j].GetComponent<Rigidbody2D>();
                    rb.velocity = Vector2.right * Random.Range(-0.75f, 0.75f) + Vector2.up * Random.Range(4.5f, 7.5f);
                    pool[i + j].transform.position = spawnPoint.position;
                }
                remainingTime -= (Time.time - startTime);
                startTime = Time.time;
                yield return null; // new WaitForSeconds(t);
            }
            clouds.Stop();
            isShooting = false;
        }
    }
}
