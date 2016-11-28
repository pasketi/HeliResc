using UnityEngine;
using System.Collections;

public class SmokeManager : MonoBehaviour {

	private Copter copterScript;
	private ParticleSystem[] smokes;
	public float maxRate = 10f;

	// Use this for initialization
	void Start () {
		copterScript = GameObject.Find ("Copter").GetComponent<Copter>();
		smokes = GetComponentsInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		foreach(ParticleSystem smoke in smokes){
            smoke.emissionRate = (((copterScript.health.maxHealth - copterScript.health.CurrentHealth) / copterScript.health.maxHealth) * maxRate);
		}
	}
}
