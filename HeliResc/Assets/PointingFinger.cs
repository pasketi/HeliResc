using UnityEngine;
using System.Collections;

public class PointingFinger : MonoBehaviour {

    private SpriteRenderer fingerImage;
    private Transform _transform;

	void OnEnable() {
		EventManager.StartListening ("EnterPlatform", EnterPlatform);
		EventManager.StartListening ("ExitPlatform", ExitPlatform);
	}
	void OnDisable() {
		EventManager.StopListening ("EnterPlatform", EnterPlatform);
		EventManager.StopListening ("ExitPlatform", ExitPlatform);
	}

    void Start() {
        _transform = transform;
        fingerImage = GetComponentInChildren<SpriteRenderer>();
        fingerImage.enabled = false;
    }

	private void EnterPlatform() {
		fingerImage.enabled = true;
	}
	public void ExitPlatform() {
		fingerImage.enabled = false;
	}
}
