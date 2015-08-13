using UnityEngine;
using System.Collections;
using System;

public class FloatingObject : MonoBehaviour {
   
    private Transform water;                            //Transform of the water layer
    private Transform _transform;                       //reference to own transform
    private float scale;                                //Half the height of water layer
    private float floatyValue;
    private bool inWater;
    private Rigidbody2D _rigidbody;

    private Action UpdateMethod = () => { };            //Assign a static or non static method to update the object

    public float scaleMultiplier = 1.25f;               //Multiplier to raise the object above water
    public bool isStatic;                               //Should the object use physics or is it static
    public bool IsInWater { get { return inWater; } }   //Getter for the inWater property

	// Use this for initialization
	void Start () {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        water = GameObject.FindGameObjectWithTag("waterLayer1").transform;
        scale = water.localScale.y * scaleMultiplier;


        if (isStatic == true) {
            inWater = true;
            UpdateMethod = StaticUpdate;
        }
        else {
            floatyValue = GetComponent<Rigidbody2D>().mass * 30f;
            UpdateMethod = NonStaticUpdate;
        }
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Floating");
        UpdateMethod();
	}

    private void StaticUpdate() {
        Vector3 pos = _transform.position;
        pos.y = water.position.y + scale;
        _transform.position = pos;
    }
    private void NonStaticUpdate() {
        //if (_transform.position.y < 0f) {
        //    _rigidbody.AddForce(Vector3.up * floatyValue);
        //}
        if (_transform.position.y < water.position.y + scale) {
            inWater = true;
            _rigidbody.AddForce(Vector3.up * floatyValue);
        }
        else if (_transform.position.y > water.position.y + scale * 2) { 
        
        }
    }

	public void ChangeToStatic(bool isStatic) {
		this.isStatic = isStatic;
		if (isStatic == true) {
			inWater = true;
			UpdateMethod = StaticUpdate;
		}
		else {
			floatyValue = GetComponent<Rigidbody2D>().mass * 30f;
			UpdateMethod = NonStaticUpdate;
		}
	}
}