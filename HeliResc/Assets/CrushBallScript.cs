using UnityEngine;
using System.Collections;

public class CrushBallScript : MonoBehaviour {

    public GameObject ballPrefab;
    public float ballDistance = 4;
    public float ballSnapDistance = 6;

	// Use this for initialization
	void Start () {
        GameObject.Find("Copter").GetComponent<Copter>().rope.CreateHook(ballPrefab, ballDistance, ballSnapDistance);
	}


}
