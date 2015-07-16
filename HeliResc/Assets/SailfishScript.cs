using UnityEngine;
using System.Collections;

public class SailfishScript : MonoBehaviour {

    public GameObject waterSplash;      //Instantiate a splash when the fish hits water level
    public float jumpForce;             //The force how fast the fish will jump

    private Copter copter;              //Reference to player
    private Rigidbody2D _rigidbody;     //Reference to rigidbody component
    private Transform _transform;

    private float waterLevel;           //The water level of the level, found from level manager    
    private bool splashed;              //Have the fish already splashed

    private Vector2 forceVector;        //A vector to add force to the fish, takes the jumpforce variable as y-component
    private float startPosition;        //The starting point from which the fish should start the jump

    void Start() {
        copter = GameObject.Find("Copter").GetComponent<Copter>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = transform;
        forceVector = new Vector2(0, jumpForce);

        waterSplash = Instantiate(waterSplash, Vector3.zero, Quaternion.identity) as GameObject;
        waterLevel = GameObject.FindObjectOfType<LevelManager>().getWaterLevel();

        splashed = true;
        startPosition = _transform.position.y;
    }

    void Update() {
        //Rotate the fish according its direction, pointing either up or down
        Vector3 rotation = new Vector3(0, 0, -90);
        rotation.z *= Mathf.Sign(_rigidbody.velocity.y);

        _transform.rotation = Quaternion.Euler(rotation);
        if (splashed == false) {
            if (_transform.position.y < waterLevel && _rigidbody.velocity.y < 0) {
                Debug.Log("Splash");
                Splash();
            }
        }
        if (_transform.position.y < startPosition)
            Jump();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        string tag = collision.gameObject.tag;
        if (tag.Equals("Copter")) {
            copter.Detonate();
        }
        else if (tag.Equals("Hook")) {
            copter.rope.KillHook();
        }
    }
    private void Splash() {
        splashed = true;
        waterSplash.transform.position = new Vector3(_transform.position.x, waterLevel);
        waterSplash.GetComponent<Animator>().Play("Splash");
    }
    private void Jump() {
        splashed = false;
        _rigidbody.velocity = forceVector;
    }
}
