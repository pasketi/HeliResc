﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WhaleScript : MonoBehaviour {

    public Transform spawnPoint;
    public GameObject waterDrop;
    public int amountPerSecond;
    public float time;

    private List<GameObject> pool;

	// Use this for initialization
	void Start () {
        pool = new List<GameObject>();

        int a = (int)(time * amountPerSecond);
        for (int i = 0; i < a; i++) {
            GameObject g = Instantiate(waterDrop) as GameObject;
            g.transform.parent = spawnPoint;
            g.SetActive(false);
            pool.Add(g);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
            Launch();
	}

    void Launch() {
        StartCoroutine(Launcher());
    }

    IEnumerator Launcher() {
        int amount = 3;
        float t = time / pool.Count * amount;
        float startTime = Time.time;
        float remainingTime = time;
        Debug.Log("Start " + startTime);
        for (int i = 0; i < pool.Count; i += amount)
        {
            for (int j = 0; j < amount; j++)
            {
                pool[i+j].SetActive(true);
                Rigidbody2D rb = pool[i + j].GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.right * Random.Range(-0.75f, 0.75f);
                pool[i + j].transform.position = spawnPoint.position;                
            }
            remainingTime -= (Time.time - startTime);
            startTime = Time.time;
            yield return null; // new WaitForSeconds(t);
        }
        Debug.Log("End " + Time.time);
    }
}