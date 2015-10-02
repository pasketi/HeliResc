using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BurningObject : ActionableObject {

    [HideInInspector]
    public bool dead;                           //Is the object still saveable
	public bool burning;
    public int startState = 1;                  //Which state the objects starts burning.
	public float burnStateTime;				    //The time it will take from the object to advance to next level of burning
	public Sprite[] burnSprites;			    //The sprites of different states of the burning object. Assign the most healthy to the first slot
	public List<GameObject> fires;			    //List of bigger fire gameobjects on the object
	public List<GameObject> smallFires;		    //list of the small fire gameobject on the object

	private System.Action UpdateMethod;


	private SpriteRenderer _sprite;			    //Reference to sprite renderer component
    private List<BurningObject> nearbyObjects;  //List of objects that are close enough to be ignited
    private LevelManager manager;

    private bool saved;
	private bool ignite;					    //Should the object ignite nearby objects
	private int burnState;					    //The current state of burning. Maximum value is determined by the lenght of burning sprites array
	private int maxState;					    //Max value of the burnstate variable
	private float damage;					    //A timer to keep track of how long the object has been burning


	// Use this for initialization
	void Awake () {
		_sprite = GetComponent<SpriteRenderer> ();
		_sprite.sprite = burnSprites [0];
		burnState = 1;
		maxState = burnSprites.Length;

        manager = GameObject.FindObjectOfType<LevelManager>();

        UpdateMethod = () => { };        
        if (burning == true)
        {
            StartBurning();
        }
        else
            StopBurning();
        UpdateState();
        FindNearbyObjects();
	}    
   
	// Update is called once per frame
	void Update () {
        UpdateMethod ();
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Water")) {
            UseAction();
        }
    }

	public void StartBurning() {
        if (dead == false && saved == false)
        {
            UpdateMethod = Burn;
            burning = true;
        }
	}
	public void StopBurning() {
		UpdateMethod = () => { };
        SetBigFire(false);
        SetSmallFire(false);
		burning = false;
	}
    public override void UseAction()
    {
        if (saved == true) return;
        StopBurning();
        saved = true;
        manager.saveCrates(1);
    }
	private void Burn() {
		damage += Time.deltaTime;        
		if (damage >= burnState * burnStateTime) {
			NextState();            
		} 
	}
	private void NextState() {
        
        if(burnState < maxState)
		    _sprite.sprite = burnSprites [burnState];   //Set correct sprite to the object

        //ignite = true;
        IgniteNext();

        burnState++;
        UpdateState();
	}
    private void UpdateState() {
        switch (burnState)
        {
            case 1:
                SetSmallFire(false);
                SetBigFire(false);
                break;
            case 2:
                SetSmallFire(true);
                break;
            case 3:
                SetBigFire(true);
                break;
            case 4:
                Die();
                break;
        }
    }
    private void SetSmallFire(bool active = true) {
        foreach (GameObject go in smallFires)
        {
            go.SetActive(active);
            if(active == true)
                StartCoroutine(Tutorial.FadeIn(go.GetComponent<SpriteRenderer>(), 1));
            /*
            if(animate == true)
                StartCoroutine(SetScale(go.transform, 1, !active));
            else
                go.SetActive(active);*/
        }
    }
    private void SetBigFire(bool active = true) {
        foreach (GameObject go in fires)
        {

            go.SetActive(active);
            if(active == true)
                StartCoroutine(Tutorial.FadeIn(go.GetComponent<SpriteRenderer>(), 1));
            /*
            if(animate == true)
                StartCoroutine(SetScale(go.transform, 1, !active));
            else
                go.SetActive(active);*/
        }        
    }
	private void Die() {
        burning = false;
        dead = true;
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols) {
            if (c.isTrigger == false)
                c.enabled = false;
        }
        //_sprite.sprite = burnSprites[maxState - 1];
        StopBurning();
	}
    private void FindNearbyObjects() {
        var objs = GameObject.FindObjectsOfType<BurningObject>();
        nearbyObjects = new List<BurningObject>();

        CircleCollider2D cc = GetComponent<CircleCollider2D>();
        float radius = cc.radius;
        Destroy(cc);

        foreach(BurningObject bo in objs) {
            if(Vector3.Distance(bo.transform.position, transform.position) < (radius * 2))
                nearbyObjects.Add(bo);
        }
    }
    private void IgniteNext() { 
        foreach(BurningObject bo in nearbyObjects) {
            if (bo.burning == false && bo.dead == false) {
                bo.StartBurning();
                break;
            }
        }
    }

    private IEnumerator SetScale(Transform tr, float time, bool shrink) {
        float t = 1 / time;
        Vector3 vec = tr.localScale;        

        if (shrink == true) {
            while (vec.x > 0)
            {
                float f = Time.deltaTime * t;
                vec.x -= f;
                vec.y -= f;
                tr.localScale = vec;
                yield return null;
            }

            vec.x = vec.y = 0;
            tr.localScale = vec;
            tr.gameObject.SetActive(false);
        }
        else
        {
            tr.gameObject.SetActive(true);
            tr.localScale = Vector3.zero;
            while (vec.x < 1)
            {
                float f = Time.deltaTime * t;
                vec.x += f;
                vec.y += f;
                tr.localScale = vec;
                yield return null;
            }

            vec.x = vec.y = 1;
            tr.localScale = vec;
        }

    }
}
