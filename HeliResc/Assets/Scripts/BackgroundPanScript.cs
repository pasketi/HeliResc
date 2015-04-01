using UnityEngine;
using System.Collections;

public class BackgroundPanScript : MonoBehaviour {

	private float cameraMinY, cameraMaxY;
	private Skybox sky;
	private float y;
	public float yMin = 0.1f, yMax = 0.5f;

	// Use this for initialization
	void Start () {
		sky = GetComponent<Skybox>();
		cameraMinY = transform.position.y;
		if (GetComponent<CameraManager>() != null) cameraMaxY = GetComponent<CameraManager>().maxY;
		else cameraMaxY = 30f;
		sky.material.SetTextureOffset("_FrontTex", new Vector2 (0f,yMin));
	}
	
	// Update is called once per frame
	void Update () {
		y = ((yMax * (cameraMinY - transform.position.y)) + (yMin*(transform.position.y-cameraMaxY)))/(cameraMinY-cameraMaxY);
		sky.material.SetTextureOffset("_FrontTex", new Vector2 (0f,y));
	}
}
