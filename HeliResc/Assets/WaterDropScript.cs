using UnityEngine;
using System.Collections;

public class WaterDropScript : MonoBehaviour {

    public float timeToLive;
    private float time;
    private SpriteRenderer sr;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable() {
        time = 1/timeToLive;
        Color c = sr.color;
        c.a = 1;
        sr.color = c;
    }

    void Update() {
        Color c = sr.color;

        if (c.a > 0) {
            c.a -= time * Time.deltaTime;
            sr.color = c;
        } else {
            gameObject.SetActive(false);
        }
    }
}
