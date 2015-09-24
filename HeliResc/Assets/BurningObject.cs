using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BurningObject : ActionableObject {

    [HideInInspector]
    public bool dead;                       //Is the object still saveable
	public bool burning;
    public int startState = 1;              //Which state the objects starts burning.
	public float burnStateTime;				//The time it will take from the object to advance to next level of burning
	public Sprite[] burnSprites;			//The sprites of different states of the burning object. Assign the most healthy to the first slot
	public List<GameObject> fires;			//List of bigger fire gameobjects on the object
	public List<GameObject> smallFires;		//list of the small fire gameobject on the object

	private System.Action UpdateMethod;


	private SpriteRenderer _sprite;			//Reference to sprite renderer component

    private bool saved;
	private bool ignite;					//Should the object ignite nearby objects
	private int burnState;					//The current state of burning. Maximum value is determined by the lenght of burning sprites array
	private int maxState;					//Max value of the burnstate variable
	private float damage;					//A timer to keep track of how long the object has been burning


	// Use this for initialization
	void Awake () {
		_sprite = GetComponent<SpriteRenderer> ();
		_sprite.sprite = burnSprites [0];
		burnState = 1;
		maxState = burnSprites.Length;

        UpdateMethod = () => { };
        if (burning == true)
        {
            StartBurning();
        }
        else
            StopBurning();
        UpdateState();

	}    
   
	// Update is called once per frame
	void Update () {
        Debug.Log("Update Object " + gameObject.name + " is burning: " + burning);
        UpdateMethod ();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.CompareTag ("Burn")) {
            BurningObject bo = other.GetComponent<BurningObject>();

            if (ignite == true && bo.burning == false && bo.dead == false) {
                bo.StartBurning();
                ignite = false;
            }
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
        saved = true;
        GameObject.FindObjectOfType<LevelManager>().saveCrates(1);
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

        ignite = true;
        burnState++;
        UpdateState();
	}
    private void UpdateState() {
        switch (burnState)
        {
            case 2:
                SetSmallFire();
                break;
            case 3:
                SetBigFire();
                break;
            case 4:
                Die();
                break;
        }
    }
    private void SetSmallFire(bool active = true) {
        foreach (GameObject go in smallFires)
            go.SetActive(active);
    }
    private void SetBigFire(bool active = true) {
        foreach (GameObject go in fires)
            go.SetActive(active);
    }
	private void Die() {
        burning = false;
        dead = true;
        //_sprite.sprite = burnSprites[maxState - 1];
        StopBurning();
	}    
}
