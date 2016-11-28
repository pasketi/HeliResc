using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForestFloor : MonoBehaviour {

    public GameObject puddlePrefab;
    public int sheepGroup;

    private List<SheepScript> sheeps;
    private List<GameObject> puddles;
    private int amount = 5;

    void Start() {
        
        puddles = new List<GameObject>();
        sheeps = new List<SheepScript>();

        for (int i = 0; i < amount; i++) {
            GameObject go = Instantiate(puddlePrefab) as GameObject;
            puddles.Add(go);
            go.SetActive(false);
        }
        SheepScript[] ss = GameObject.FindObjectsOfType<SheepScript>();

        foreach (SheepScript s in ss)
        {
            if (s.group.Equals(sheepGroup))
                sheeps.Add(s);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Water")) {
            CreatePuddle(other.transform.position);
            //other.gameObject.SetActive(false);
        }
    }

    private void CreatePuddle(Vector3 position) {
        GameObject go = null;

        if (puddles.Count > 0)
        {
            go = puddles[0];
            go.SetActive(true);
            puddles.Remove(go);
        }
        else { 
            go = Instantiate(puddlePrefab) as GameObject; 
        }

        Vector3 pos = new Vector3(position.x, transform.position.y);
        go.transform.position = pos;
        StartCoroutine(Tutorial.FadeIn(go.GetComponent<SpriteRenderer>(), 1));

        if(sheeps.Count > 0) {
            sheeps[Random.Range(0, sheeps.Count)].RunToWater(go.transform);
        }
        
    }   
}
