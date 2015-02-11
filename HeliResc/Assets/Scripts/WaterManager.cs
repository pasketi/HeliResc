using UnityEngine;
using System.Collections;

public class WaterManager : MonoBehaviour {

	public float 	wave0Magnitude = 1f, wave0Amplitude = 1f, wave0TimeOffset = 0.00f, wave0Speed = 1f,
					wave1Magnitude = 1f, wave1Amplitude = 1f, wave1TimeOffset = 0.30f, wave1Speed = 1f,
					wave2Magnitude = 1f, wave2Amplitude = 1f, wave2TimeOffset = 0.65f, wave2Speed = 1f,
					generalScale = 0.3f;
	private Vector3 waterLayer0, waterLayer1, waterLayer2; //Offsets
	private GameObject[] layer0, layer1, layer2;
	private GameObject waterLeft, waterMid, waterRight;
	public Sprite exampleWaterSprite;

	// Use this for initialization
	void Start () {
		layer0 = GameObject.FindGameObjectsWithTag("waterLayer0");
		layer1 = GameObject.FindGameObjectsWithTag("waterLayer1");
		layer2 = GameObject.FindGameObjectsWithTag("waterLayer2");
		
		waterLeft = GameObject.FindGameObjectWithTag("waterLeft");
		waterMid = GameObject.FindGameObjectWithTag("waterMid");
		waterRight = GameObject.FindGameObjectWithTag("waterRight");
		setLeftAndRightWaters();
	}
	
	// Update is called once per frame
	void Update () {
		waterLayer0 = new Vector3(Mathf.Cos((Time.time + wave0TimeOffset) * wave0Speed) * wave0Amplitude * generalScale, 
		                          Mathf.Sin((Time.time + wave0TimeOffset) * wave0Speed) * wave0Magnitude * generalScale);
		waterLayer1 = new Vector3(Mathf.Cos((Time.time + wave1TimeOffset) * wave1Speed) * wave1Amplitude * generalScale, 
		                          Mathf.Sin((Time.time + wave1TimeOffset) * wave1Speed) * wave1Magnitude * generalScale);
		waterLayer2 = new Vector3(Mathf.Cos((Time.time + wave2TimeOffset) * wave2Speed) * wave2Amplitude * generalScale, 
		                          Mathf.Sin((Time.time + wave2TimeOffset) * wave2Speed) * wave2Magnitude * generalScale);

		foreach (GameObject layer in layer0){
			layer.transform.localPosition = waterLayer0;
		}
		foreach (GameObject layer in layer1){
			layer.transform.localPosition = waterLayer1;
		}
		foreach (GameObject layer in layer2){
			layer.transform.localPosition = waterLayer2;
		}

		if (Camera.main.transform.position.x > (waterMid.transform.position.x + (exampleWaterSprite.texture.width/exampleWaterSprite.pixelsPerUnit)/2)){
			gameObject.transform.position = waterRight.transform.position;
		} else if (Camera.main.transform.position.x < (waterMid.transform.position.x - (exampleWaterSprite.texture.width/exampleWaterSprite.pixelsPerUnit)/2)) {
			gameObject.transform.position = waterLeft.transform.position;
		}
	}

	private void setLeftAndRightWaters() {
		waterLeft.transform.localPosition = new Vector3(-(exampleWaterSprite.texture.width/exampleWaterSprite.pixelsPerUnit), waterLeft.transform.localPosition.y);
		waterRight.transform.localPosition = new Vector3(exampleWaterSprite.texture.width/exampleWaterSprite.pixelsPerUnit, waterRight.transform.localPosition.y);
	}
}
