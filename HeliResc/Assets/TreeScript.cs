using UnityEngine;
using System.Collections;

public class TreeScript : MonoBehaviour {

    void Awake() {
        HingeJoint2D hinge = GetComponent<HingeJoint2D>();
        hinge.connectedAnchor = ((Vector2)transform.position) + hinge.anchor;
    }
}
