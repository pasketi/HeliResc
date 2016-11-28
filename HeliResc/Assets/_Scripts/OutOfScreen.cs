using UnityEngine;
using System.Collections;

public class OutOfScreen : MonoBehaviour {

	// Use this for initialization
	void OnBecameInvisible(){
		Destroy (gameObject);
	}
}
