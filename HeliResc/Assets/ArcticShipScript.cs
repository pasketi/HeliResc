using UnityEngine;
using System.Collections;

public class ArcticShipScript : MonoBehaviour {

    public Transform target;
    public float speed;
    public Sprite damagedShip;

    private SpriteRenderer _sprite;
    private Vector3 speedVector;
    private Transform _transform;
    private bool gotHit = false;
    private bool move;

	// Use this for initialization
	void Start () {
        _transform = transform;
        _sprite = GetComponent<SpriteRenderer>();

        //If the target is left from the ship, change direction to left
        if (target.position.x < _transform.position.x) {
            if (speed > 0) speed *= -1;
        }

        Vector3 scale = _transform.localScale;
        if (speed > 0) scale.x *= -1;
        _transform.localScale = scale;

        speedVector = new Vector3(speed * Time.fixedDeltaTime,0);
        Activate();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(move == true)
            MoveToTarget();
	}
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("collision: " + other.name);
        SeracScript serac = other.GetComponent<SeracScript>();
        if (serac != null) {
            serac.UseAction();
            if (gotHit == true)
                StartCoroutine(Sink());
            else HitSerac();
        }
    }

    public void Activate() {
        move = true;
    }
    private IEnumerator Sink() {
        GetComponent<FloatingObject>().enabled = false;        
        while (_transform.eulerAngles.z < 45) {
            _transform.Rotate(new Vector3(0, 0, 22.5f * Time.deltaTime));
            yield return null;
        }
        move = false;
    }
    private void HitSerac() {
        _sprite.sprite = damagedShip;
        gotHit = true;
    }
    private void MoveToTarget() {
        _transform.Translate(speedVector);
    }        
}
