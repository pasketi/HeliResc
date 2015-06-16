using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WhaleScript : MonoBehaviour {

    public Transform spawnPoint;
    public GameObject waterDrop;
    public GameObject waterDrop2;
    public int amountPerSecond;
    public float time;
    public ParticleSystem clouds;
    private List<GameObject> pool;

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
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
            Launch();
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.name.Equals("Copter")) {
            Launch();
        }
    }

    void Launch() {
        StartCoroutine(Launcher());
    }
    

    IEnumerator Launcher() {
        clouds.Play();
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
        clouds.Stop();
        Debug.Log("End " + Time.time);
    }
}
