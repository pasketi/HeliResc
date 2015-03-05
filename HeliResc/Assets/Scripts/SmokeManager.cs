using UnityEngine;
using System.Collections;

public class SmokeManager : MonoBehaviour {

	private CopterManagerTouch copterScript;
	private ParticleSystem[] smokes;
	public float maxRate = 20f;

	// Use this for initialization
	void Start () {
		copterScript = GameObject.Find ("Copter").GetComponent<CopterManagerTouch>();
		smokes = GetComponentsInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		foreach(ParticleSystem smoke in smokes){
			smoke.emissionRate = (Random.value * 0.3f) + (float)(((float)((float)copterScript.maxHealth - (float)copterScript.getHealth()) / (float)copterScript.maxHealth) * (float)maxRate);
		}
	}
}
