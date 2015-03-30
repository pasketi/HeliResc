using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArrowManager : MonoBehaviour {

	private GameObject copter, target, landingPad/*, pointOfInterest*/;
	private GameObject[] targets;

	// Use this for initialization
	void Start () {
		copter = GameObject.Find ("Copter");
		targets = GameObject.FindGameObjectsWithTag ("Crate");
		landingPad = GameObject.FindGameObjectWithTag ("LandingPad");
	}
	
	// Update is called once per frame
	void Update () {
		if (copter == null) copter = GameObject.Find ("Copter");
		if ((GameObject.Find("Hook(clone)") != null && GameObject.Find("Hook(clone)").transform.childCount <= 2) || GameObject.Find("LevelManagerO").GetComponent<LevelManager>().cargoCrates <= 2) {
			foreach (GameObject current in targets) {
				if (target == null) {
					target = current;
				} else if (Vector3.Distance(copter.transform.position, current.transform.position) < 
				           Vector3.Distance(copter.transform.position, target.transform.position)) {
					target = current;
				}
			}
			if (target.GetComponent<Renderer>().isVisible != true) {
				GetComponent<Image>().enabled = true;
				transform.LookAt(Camera.main.WorldToScreenPoint(target.transform.position));
				transform.Rotate (0, -90, 0);
			} else {
				GetComponent<Image>().enabled = false;
			}
		} else {
			if (landingPad.GetComponent<Renderer>().isVisible != true) {
				GetComponent<Image>().enabled = true;
				transform.LookAt(Camera.main.WorldToScreenPoint(landingPad.transform.position));
				transform.Rotate (0, -90, 0);
			} else {
				GetComponent<Image>().enabled = false;
			}
		}


	}
}
