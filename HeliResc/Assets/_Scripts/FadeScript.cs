﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeScript : MonoBehaviour {

    private Image blackImage;
    public float speedIn = 4;
    public float speedOut = 2;

    void OnEnable() {
        EventManager.StartListening(SaveStrings.eFadeIn, FadeIn);
        EventManager.StartListening(SaveStrings.eFadeOut, FadeOut);
    }
    void OnDisable() {
        EventManager.StopListening(SaveStrings.eFadeIn, FadeIn);
        EventManager.StopListening(SaveStrings.eFadeOut, FadeOut);
    }

	// Use this for initialization
	void Start () {
        blackImage = GetComponent<Image>();
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.K))
            EventManager.TriggerEvent(SaveStrings.eFadeIn);
        if (Input.GetKeyDown(KeyCode.L))
            EventManager.TriggerEvent(SaveStrings.eFadeOut);
    }

    private void FadeIn() {
        StartCoroutine(FadingIn());
    }

    private void FadeOut() {
        StartCoroutine(FadingOut());
    }

    private IEnumerator FadingIn() {
        Color c = blackImage.color;

        c.a = 1;
        blackImage.color = c;

        float startTime = Time.time;

        while (startTime + speedIn > Time.time)
        {
            float timeElapsed = Time.time - startTime;
            c.a = 1.0f - (timeElapsed / speedIn);
            blackImage.color = c;
            yield return new WaitForSeconds(0.01f);
        }

        c.a = 0;
        blackImage.color = c;
    }

    private IEnumerator FadingOut()
    {
        Color c = blackImage.color;

        c.a = 0;
        blackImage.color = c;

        float startTime = Time.time;

        while (startTime + speedOut > Time.time)
        {
            float timeElapsed = Time.time - startTime;
            c.a = timeElapsed / speedOut;
            blackImage.color = c;
            yield return new WaitForSeconds(0.01f);
        }
        c.a = 1;
        blackImage.color = c;
    }
}
