using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialArrowManager : MonoBehaviour {

    private Image arrowImage;    

    private TutorialPOI[] targets;
    private Transform[] positions;

    private Transform target;

    private int currentTarget = 0;

    void OnEnable() { EventManager.StartListening("NextPOI", NextTarget); }
    void OnDisable() { EventManager.StopListening("NextPOI", NextTarget); }


    void Start () {
        targets = GameObject.FindObjectsOfType<TutorialPOI>();
        positions = new Transform[targets.Length];
        for (int i = 0; i < targets.Length; i++)
            positions[targets[i].index] = targets[i].transform;

        arrowImage = GetComponent<Image>();        

        target = positions[currentTarget];
    }
	
	// Update is called once per frame
	void Update () {
        CalculateRotation(); 
	}

    private void NextTarget() {

        StartCoroutine(ShowArrow(false, 0));

        if (currentTarget < (targets.Length - 1)) {
            currentTarget++;
            target = positions[currentTarget];
        }

        StartCoroutine(ShowArrow(true, 5));
    }

    private void CalculateRotation() {
        Vector3 diff = target.position - Camera.main.ScreenToWorldPoint(transform.position);
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }

    private IEnumerator ShowArrow(bool show, float time) {
        yield return new WaitForSeconds(time);
        arrowImage.enabled = show;
    }
}
