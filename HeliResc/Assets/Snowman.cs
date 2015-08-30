using UnityEngine;
using System.Collections;

public class Snowman : MonoBehaviour {

    public float timeBetweenThrows;
    public float throwForce = 1000;       //Force to throw the ball
    public GameObject snowballPrefab;   //Prefab of the snowball to throw
    public Transform arm;

    private Transform copter;           //Reference to copters transform
    private float throwTime;

	// Use this for initialization
	void Start () {
        throwTime = Time.time;
        copter = GameObject.FindObjectOfType<Copter>().transform;        
	}

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Copter")) {
            RotateArm();
        }
    }

    private void RotateArm() {
        Vector3 rotation = arm.eulerAngles;
        rotation.z += Time.deltaTime * 180;
        arm.eulerAngles = rotation;

        float angle = Vector3.Angle(-arm.up, (copter.position - arm.position));
        float dot = Vector3.Dot(-arm.right, (copter.position - arm.position).normalized);
        if (Mathf.Abs(angle - 90) < 3 && dot < 0) {
            if(throwTime + timeBetweenThrows < Time.time)
                ThrowBall();           
        }

        Debug.Log("Angle of copter and arm: " + Vector3.Angle(-arm.up, (copter.position - arm.position)));
        Debug.Log("Dot of copter and arm: " + Vector3.Dot(-arm.right, (copter.position - arm.position).normalized));
    }

    private void ThrowBall() {

        throwTime = Time.time;
        Vector3 ballPos = arm.position - arm.up * 3;
        GameObject go = Instantiate(snowballPrefab, ballPos, Quaternion.identity) as GameObject;

        go.GetComponent<Rigidbody2D>().AddForce((copter.position - ballPos).normalized * throwForce);

    }
}
