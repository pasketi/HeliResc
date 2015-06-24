using UnityEngine;
using System.Collections;

public class FloatingObject : MonoBehaviour {
   
    private Transform water;                //Transform of the water layer
    private Transform _transform;           //reference to own transform
    private float scale;                    //Half the height of water layer

    public float scaleMultiplier = 1.25f;   //Multiplier to raise the object above water

	// Use this for initialization
	void Start () {
        _transform = transform;
        water = GameObject.FindGameObjectWithTag("waterLayer1").transform;
        scale = water.localScale.y * scaleMultiplier;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = _transform.position;
        pos.y = water.position.y + scale;
        _transform.position = pos;
	}
}