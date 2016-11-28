using UnityEngine;
using System.Collections;

public class ArcticShipScript : MonoBehaviour {

    public Transform target;
    public float speed;
    public Sprite damagedShip;
	public bool moveRight = true;

	private LevelManager manager;
    private SpriteRenderer _sprite;
    private Vector3 speedVector;
	private Vector3 targetPoint;
    private Transform _transform;
    private bool gotHit = false;
    private bool move;
	private bool stopping;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find("LevelManagerO").GetComponent<LevelManager>();
        _transform = transform;
        _sprite = GetComponent<SpriteRenderer>();

		targetPoint = target.position;

		float stopDistance = ((1640f / 300));

        //If the target is left from the ship, change direction to left
		if (targetPoint.x < _transform.position.x) {
			moveRight = false;
			targetPoint.x += stopDistance;
			if (speed > 0)
				speed *= -1;
		} else {
			moveRight = true;
			targetPoint.x -= stopDistance;
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
        //Debug.Log("collision: " + other.name);
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
	private IEnumerator Stop() {

		float sign = Mathf.Sign (speedVector.x);
		float damping = speedVector.x / 2;
		float deltaTime = Time.time;
		while(sign == Mathf.Sign (speedVector.x)) {

			speedVector.x -= damping * (Time.time - deltaTime);

			deltaTime = Time.time;
			yield return null;
		}
		move = false;
	}
    private IEnumerator Sink() {
        LevelManager lm = GameObject.FindObjectOfType<LevelManager>();
        lm.LoseLevel();
        GetComponent<FloatingObject>().enabled = false;
		if (moveRight == true) {
			while (_transform.eulerAngles.z > (360-45) || _transform.eulerAngles.z == 0) {
				_transform.Rotate (new Vector3 (0, 0, -22.5f * Time.deltaTime));
				yield return null;
			}
		} else {
			while (_transform.eulerAngles.z < 45) {
				_transform.Rotate (new Vector3 (0, 0, 22.5f * Time.deltaTime));
				yield return null;
			}
		}
        move = false;
    }
    private void HitSerac() {
        _sprite.sprite = damagedShip;
        gotHit = true;
		manager.shipNotHit = false;
    }
    private void MoveToTarget() {
        _transform.Translate(speedVector);
		if (moveRight == true && stopping == false) {
			if (_transform.position.x > targetPoint.x) {
				stopping = true;
				StartCoroutine (Stop ());
			}
		} else if (stopping == false) {
			if(_transform.position.x < targetPoint.x) {
				stopping = true;
				StartCoroutine(Stop ());
			}
		}
    }
}
