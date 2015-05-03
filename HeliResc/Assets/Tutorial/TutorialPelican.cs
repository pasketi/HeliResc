using System;
using UnityEngine;
using System.Collections;

public class TutorialPelican : MonoBehaviour {

    public GameObject pelican;

    private GameObject go;

    public ObstacleManager obstacle;

    public Action PelicanTriggered = () => { };

	// Use this for initialization
	void Start () {
        go = Instantiate(pelican) as GameObject;
        obstacle = go.GetComponent<ObstacleManager>();
        go.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name.Equals("Copter")) {
            PelicanTriggered();
            go.SetActive(true);
        }
    }
}
