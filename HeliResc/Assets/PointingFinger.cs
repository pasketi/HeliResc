using UnityEngine;
using System.Collections;

public class PointingFinger : MonoBehaviour {

    private LandingPadManager[] landings;
    private SpriteRenderer fingerImage;
    private Transform _transform;

    void Start() {
        landings = GameObject.FindObjectsOfType<LandingPadManager>();
        foreach (LandingPadManager l in landings) {
            l.enterPlatform += ShowFinger;
            l.exitPlatform += HideFinger;
        }
        _transform = transform;
        fingerImage = GetComponentInChildren<SpriteRenderer>();
        fingerImage.enabled = false;

    }

    private void ShowFinger(GameObject landing) {
        
        StartCoroutine(FingerCoroutine(landing.transform));
    }

    private void HideFinger(GameObject landing) {
        fingerImage.enabled = false;
    }

    private IEnumerator FingerCoroutine(Transform target) {
        yield return new WaitForSeconds(0.5f);

        Transform t = FindLastTransform(target);
                
        Vector3 vec = new Vector3(1, 1, 0).normalized;

        _transform.position = (t.position + (vec * 3));
        if (t.name.Equals("Position"))
            fingerImage.enabled = true;
    }

    private Transform FindLastTransform(Transform t) {       
        Transform tr = t;

        foreach (Transform trs in tr.GetComponentsInChildren<Transform>()) {
            if (trs.name.Equals("Position"))
                tr = trs;
            Debug.Log("Name of : " + trs.name);
        }

        return tr;
    }
}
