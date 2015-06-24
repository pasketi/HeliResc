using UnityEngine;
using System.Collections;

public class SnowfallScript : MonoBehaviour {

    public int index;
    private Transform levelCamera;
    private Transform _transform;
    private float screenWidth;
    private float cameraPreviousX;

	// Use this for initialization
	void Start () {
        levelCamera = GameObject.FindObjectOfType<Camera>().transform;
        _transform = transform;
        CalculateScale();
        CalculatePosition();

        screenWidth = transform.localScale.x * 2;
        cameraPreviousX = levelCamera.position.x;
	}

    void Update() {
        Vector3 vec = levelCamera.position;
        if (Vector2.Distance(new Vector2(vec.x, _transform.position.y), _transform.position) >= screenWidth) {
            Vector3 newPos;
            if (cameraPreviousX >= vec.y) {
                newPos = vec - (Vector3.right * screenWidth);
            } else
                newPos = vec + (Vector3.right * screenWidth);
            _transform.position = newPos;
        }
        cameraPreviousX = vec.x;
    }

    private void CalculateScale() {

        Vector3 left = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 right = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0));

        float x = Vector3.Distance(left, right) * 0.5f;
        
        transform.localScale = new Vector3(x, transform.localScale.y);
    }

    private void CalculatePosition() {
        
        Vector3 top = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        transform.position = new Vector3(levelCamera.position.x + transform.localScale.x * 2 * index, top.y);

    }

}
