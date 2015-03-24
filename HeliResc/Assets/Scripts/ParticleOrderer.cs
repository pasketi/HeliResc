using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ParticleSystem))]

public class ParticleOrderer : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();
		ParticleSystem particleSystem = GetComponent<ParticleSystem> ();
		particleSystem.GetComponent<Renderer>().sortingLayerID = spriteRenderer.sortingLayerID;
		particleSystem.GetComponent<Renderer>().sortingOrder = spriteRenderer.sortingOrder;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
