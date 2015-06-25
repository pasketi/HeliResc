using System;
using UnityEngine;
using System.Collections;

public class TutorialPelican : MonoBehaviour {

    public GameObject pelican;

    private GameObject go;

    public ObstacleManager obstacle;

    public Action PelicanTriggered = () => { };

	// Use this for initialization
	void Awake () {
        go = Instantiate(pelican, transform.position, Quaternion.identity) as GameObject;
        obstacle = go.GetComponent<ObstacleManager>();
        go.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name.Equals("Copter")) {
            PelicanTriggered();
            if(go != null)
                go.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
