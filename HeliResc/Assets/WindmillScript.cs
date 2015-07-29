using UnityEngine;
using System.Collections;

public class WindmillScript : MonoBehaviour {

    private HingeJoint2D hinge;     //Reference to the hinge joint 2d component of the wind mill

	// Use this for initialization
	void Start () {
        hinge = GetComponent<HingeJoint2D>();       //Assign a value to the hinge variable

        hinge.connectedAnchor = transform.position + Vector3.up * hinge.anchor.y; //Set the position of the center of the rotor
	}	
}
