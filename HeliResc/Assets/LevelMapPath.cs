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

    void Awake() {

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

        startPosition = startPoint.anchoredPosition;

        endPosition = endPoint.anchoredPosition;

        direction = (endPosition - startPosition).normalized;
        startPosition += direction * (startPointRadius + pathInterval);
        endPosition -= direction * (endPointRadius + pathInterval);

        distance = Vector3.Distance(startPosition, endPosition);
        amount = distance / pathInterval;

        LevelSetHandler set = endPoint.GetComponent<LevelSetHandler>();
        if (set.Set.unlocked && set.Set.animated) {
            SetUnlocked();
        }
        else if (set.Set.unlocked) {
            StartCoroutine(SetUnlockedAnimated(set));
        }
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
        yield return new WaitForSeconds(1);
        float speed = 6;
        float time = (1 / speed) * amount;

        StartCoroutine(GameObject.FindObjectOfType<LevelMapScript>().MoveToNextSet(startPoint, endPoint, time));

        for (int i = 0; i < amount; i++) {
            GameObject go = Instantiate(pathBallPrefab) as GameObject;
            RectTransform r = go.transform as RectTransform;
            r.SetParent(transform);
            r.localScale = Vector3.one;

            Vector3 pos = startPosition + direction * pathInterval * i;
            r.anchoredPosition = pos;

            yield return StartCoroutine(AnimateAlpha(go.GetComponent<Image>(), speed));

        }
        set.OpenSetFirstTime();
    }

    private IEnumerator AnimateAlpha(Image image, float speed) {
        Color c = image.color;
        c.a = 0;
        image.color = c;

        image.GetComponent<SoundObject>().PlaySound();        

        while (image.color.a < 1) {
            c = image.color;
            c.a += speed * Time.deltaTime;
            image.color = c;

            yield return null;
        }
    }
}