using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelMapPath : MonoBehaviour {

    public RectTransform startPoint;        //The rect transform of the starting point of the path
    public RectTransform endPoint;          //The Rect transform of the end point of the path
    public GameObject pathBallPrefab;       //The image to show on the path

    private float pathInterval;             //The distance between the dots in the path
    private float startPointRadius;
    private float endPointRadius;
    private float distance;
    private float amount;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 direction;

    void Start() {

        pathInterval = 50;  //Need to be replaced to adjust for different screen sizes

        float mapSize = GameObject.FindObjectOfType<LevelMapScript>().size;
        //Calculate the panels width and height.
        Vector2 screen = (new Vector2(Screen.width, Screen.height) * mapSize);

        startPointRadius = startPoint.sizeDelta.x;
        if (startPoint.sizeDelta.y < startPointRadius) startPointRadius = startPoint.sizeDelta.y;
        startPointRadius *= .5f;

        endPointRadius = endPoint.sizeDelta.x;
        if (endPoint.sizeDelta.y < endPointRadius) endPointRadius = endPoint.sizeDelta.y;
        endPointRadius *= .5f;

        startPosition = new Vector3();
        startPosition.x = (startPoint.anchorMin.x - .5f) * screen.x;
        startPosition.y = (startPoint.anchorMin.y - .5f) * screen.y;

        endPosition = new Vector3();
        endPosition.x = (endPoint.anchorMin.x - .5f) * screen.x;
        endPosition.y = (endPoint.anchorMin.y - .5f) * screen.y;

        direction = (endPosition - startPosition).normalized;
        startPosition += direction * (startPointRadius + pathInterval);
        endPosition -= direction * (endPointRadius + pathInterval);

        distance = Vector3.Distance(startPosition, endPosition);
        amount = distance / pathInterval;

        LevelSetHandler set = endPoint.GetComponent<LevelSetHandler>();
        //if (set.Set.unlocked && set.Set.animated) {
        //    SetUnlocked();
        //}
        //else if (set.Set.unlocked) {
            StartCoroutine(SetUnlockedAnimated(set));
        //}
    }

    private void SetUnlocked() {        
        for (int i = 0; i < amount; i++) {
            GameObject go = Instantiate(pathBallPrefab) as GameObject;
            RectTransform r = go.transform as RectTransform;
            r.SetParent(transform);
            r.localScale = Vector3.one;

            Vector3 pos = startPosition + direction * pathInterval * i;
            r.localPosition = pos;
        }
    }
    private IEnumerator SetUnlockedAnimated(LevelSetHandler set) {
        for (int i = 0; i < amount; i++) {
            GameObject go = Instantiate(pathBallPrefab) as GameObject;
            RectTransform r = go.transform as RectTransform;
            r.SetParent(transform);
            r.localScale = Vector3.one;

            Vector3 pos = startPosition + direction * pathInterval * i;
            r.localPosition = pos;

            yield return StartCoroutine(AnimateAlpha(go.GetComponent<Image>()));

        }
        set.OpenSetFirstTime();
    }

    private IEnumerator AnimateAlpha(Image image) {
        Color c = image.color;
        c.a = 0;
        image.color = c;

        image.GetComponent<SoundObject>().PlaySound();

        float speed = 6;

        while (image.color.a < 1) {
            c = image.color;
            c.a += speed * Time.deltaTime;
            image.color = c;

            yield return null;
        }
    }
}