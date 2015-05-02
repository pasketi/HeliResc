using UnityEngine;
using System.Collections;

public class TutorialPelican : MonoBehaviour {

    public GameObject pelican;

    private GameObject go;

	// Use this for initialization
	void Start () {
        go = Instantiate(pelican) as GameObject;
        go.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name.Equals("Copter"))
            go.SetActive(true);
    }
}
