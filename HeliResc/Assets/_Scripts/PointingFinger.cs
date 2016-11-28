using UnityEngine;
using System.Collections;

public class PointingFinger : MonoBehaviour {

    private SpriteRenderer fingerImage;
    private Transform _transform;

	void OnEnable() {
		EventManager.StartListening (SaveStrings.eEnterPlatform, EnterPlatform);
		EventManager.StartListening (SaveStrings.eExitPlatform, ExitPlatform);
	}
	void OnDisable() {
		EventManager.StopListening (SaveStrings.eEnterPlatform, EnterPlatform);
		EventManager.StopListening (SaveStrings.eExitPlatform, ExitPlatform);
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
