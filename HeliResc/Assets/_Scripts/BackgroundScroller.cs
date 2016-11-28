using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

    public Transform left;                  //Reference to the transform of the background
    public Transform mid;
    public Transform right;

    private Vector3 camPos;
    private float bgWidth = 40.96f;         //The width of one background element in unity units times 2.
    private Transform camera;
	// Use this for initialization
	void Start () {
        camera = Camera.main.transform;
        camPos = new Vector3(camera.position.x, mid.position.y);
	}
	
	// Update is called once per frame
	void Update () {
        UpdatePositions();
	}

    private void UpdatePositions() {
        camPos.x = camera.position.x;
        Transform furthest = left;
        if (Vector3.Distance(camPos, mid.position) > Vector3.Distance(camPos, furthest.position))
            furthest = mid;
        if (Vector3.Distance(camPos, right.position) > Vector3.Distance(camPos, furthest.position))
            furthest = right;
        float distance = Vector3.Distance(camPos, furthest.position);
        if (distance >= bgWidth) {
            if (furthest.position.x < camPos.x)
            {
                furthest.position += Vector3.right * 61.44f;
            }
            else {
                furthest.position -= Vector3.right * 61.44f;
            }
        }

    }
}
