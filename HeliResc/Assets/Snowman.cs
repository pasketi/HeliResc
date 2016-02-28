using UnityEngine;
using System.Collections;

public class Snowman : MonoBehaviour {

    public float timeBetweenThrows;
    public float throwForce = 1000;         //Force to throw the ball
    public GameObject snowballPrefab;       //Prefab of the snowball to throw
    public Transform arm;                   //The arm that rotates around the snowman
    public Transform hand;                  //Place to add a snowball

    private Transform copter;           //Reference to copters transform
    private float throwTime;
    public bool swinging = false;

	// Use this for initialization
	void Start () {
        throwTime = Time.time;
        copter = GameObject.FindObjectOfType<Copter>().transform;        
	}

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Copter") && swinging == false) {
            StartCoroutine(Swing());
        }
    }

    private IEnumerator Swing() {
        swinging = true;
        GameObject ball = Instantiate(snowballPrefab, hand.position, Quaternion.identity) as GameObject;
        ball.transform.SetParent(hand);
        ball.transform.localPosition = Vector3.zero;
        //Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        //rb.gravityScale = 0;

        float stepTime = timeBetweenThrows / 100;
        float speed = 7.2f;
        float dist = 0;

        for (int i = 0; i < 100; i++ )
        {
            Vector3 rotation = arm.eulerAngles;
            rotation.z += speed;
            dist += speed;
            arm.eulerAngles = rotation;
            speed -= 0.036f;

            float angle = Vector3.Angle(-arm.up, (copter.position - arm.position));
            float dot = Vector3.Dot(-arm.right, (copter.position - arm.position).normalized);
            if (Mathf.Abs(angle - 90) < 3 && dot < 0) {
                ThrowBall(ball);           
            }

            if (dist > 360) {
                break;
            }

            yield return new WaitForSeconds(stepTime);
        }
        arm.eulerAngles = Vector3.zero;
        swinging = false;
        //Debug.Log("Speed: " + speed);
        //Debug.Log("Dist: " + dist);
    }    

    private void RotateArm() {
        Vector3 rotation = arm.eulerAngles;
        rotation.z += Time.deltaTime * 180;
        arm.eulerAngles = rotation;

        float angle = Vector3.Angle(-arm.up, (copter.position - arm.position));
        float dot = Vector3.Dot(-arm.right, (copter.position - arm.position).normalized);
        if (Mathf.Abs(angle - 90) < 3 && dot < 0) {
            //if(throwTime + timeBetweenThrows < Time.time)
                //ThrowBall();           
        }

        //Debug.Log("Angle of copter and arm: " + Vector3.Angle(-arm.up, (copter.position - arm.position)));
        //Debug.Log("Dot of copter and arm: " + Vector3.Dot(-arm.right, (copter.position - arm.position).normalized));
    }

    private void ThrowBall(GameObject ball) {

        ball.GetComponent<Collider2D>().enabled = true;
        ball.transform.SetParent(null);
        
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.AddForce((copter.position - ball.transform.position).normalized * throwForce);
        rb.gravityScale = 1;
        


        //go.GetComponent<Rigidbody2D>().AddForce((copter.position - ballPos).normalized * throwForce);

    }
}
