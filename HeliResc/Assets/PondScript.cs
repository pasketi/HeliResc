using UnityEngine;
using System.Collections;

public class PondScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        BucketScript bucket = other.GetComponent<BucketScript>();
        //Debug.Log("Collision: " + other.name);
        if (bucket != null) { 
            //TODO
            bucket.Fill();
        }
    }
}
