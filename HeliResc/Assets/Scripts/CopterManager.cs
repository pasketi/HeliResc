using UnityEngine;
using System.Collections;

public class CopterManager : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	public Sprite copterSprite1, copterSprite2;
	float lastTime = 0f, frameTime = 0.125f;
	
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
		Debug.Log(spriteRenderer.gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
		if (lastTime < Time.time - frameTime){
			if (spriteRenderer.sprite.name == copterSprite1.name) spriteRenderer.sprite = copterSprite2;
			else if (spriteRenderer.sprite.name == copterSprite2.name) spriteRenderer.sprite = copterSprite1;
			lastTime = Time.time;
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow) && gameObject.transform.rotation.y != 180f){
			//gameObject.transform.RotateAround(gameObject.transform.position, gameObject.transform.up, 180f);
			gameObject.transform.localEulerAngles = new Vector3 (gameObject.transform.rotation.x, 180f, gameObject.transform.rotation.z);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) && gameObject.transform.rotation.y != 0f){
			//gameObject.transform.RotateAround(gameObject.transform.position, gameObject.transform.up, 180f);
			gameObject.transform.localEulerAngles = new Vector3 (gameObject.transform.rotation.x, 0f, gameObject.transform.rotation.z);
		}
	}
}
