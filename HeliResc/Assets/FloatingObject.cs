using UnityEngine;
using System.Collections;
using System;

public class FloatingObject : MonoBehaviour {
   
    private Transform water;                //Transform of the water layer
    private Transform _transform;           //reference to own transform
    private float scale;                    //Half the height of water layer
    private float floatyValue;

    private Action UpdateMethod = () => { };//Assign a static or non static method to update the object

    public float scaleMultiplier = 1.25f;   //Multiplier to raise the object above water
    public bool isStatic;                   //Should the object use physics or is it static

	// Use this for initialization
	void Start () {
        _transform = transform;
        water = GameObject.FindGameObjectWithTag("waterLayer1").transform;
        scale = water.localScale.y * scaleMultiplier;


        if (isStatic == true)
            UpdateMethod = StaticUpdate;
        else {
            floatyValue = GetComponent<Rigidbody2D>().mass * 30f;
            UpdateMethod = NonStaticUpdate;
        }
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMethod();
	}

    private void StaticUpdate() {
        Vector3 pos = _transform.position;
        pos.y = water.position.y + scale;
        _transform.position = pos;
    }
    private void NonStaticUpdate() {
        if (_transform.position.y < 0f)
				GetComponent<Rigidbody2D> ().AddForce (Vector3.up * floatyValue);
        if (_transform.position.y < water.position.y + scale) { 
            
        }
    }
}