using UnityEngine;
using System.Collections;

public class Tutorial2Script : MonoBehaviour {

    public GameObject firstArrow;
    public GameObject secondArrow;

    void Start() {
        LandingPadManager[] lands = FindObjectsOfType<LandingPadManager>();
        foreach(LandingPadManager l in lands) {
            l.enterPlatform += FadeOutArrow;
        }
    }

    private void FadeOutArrow(GameObject go) {
        Debug.Log("GO name: " + go.transform.root.name);
        if (go.transform.root.name.Equals("LandingBoat")) {
            StartCoroutine(FadeOut(secondArrow, .01f));
        } else if(go.transform.root.name.Equals("FuelShip Wide")) {
            StartCoroutine(FadeOut(firstArrow, .01f));
        }
    }

    private IEnumerator FadeIn(GameObject parent, float time)
    {
        SpriteRenderer[] sprites = parent.GetComponentsInChildren<SpriteRenderer>();


        foreach (SpriteRenderer s in sprites)
        {
            Color c = s.color;
            c.a = 0;
            s.color = c;
        }
        Debug.Log("Start time: " + Time.time);
        while (sprites[0].color.a < 1)
        {
            foreach (SpriteRenderer s in sprites)
            {
                Color c = s.color;
                c.a += time;
                s.color = c;
            }
            yield return null;
        }
        Debug.Log("End time: " + Time.time);
    }
    private IEnumerator FadeOut(GameObject parent, float time)
    {
        SpriteRenderer[] sprites = parent.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer s in sprites)
        {
            Color c = s.color;
            c.a = 1;
            s.color = c;
        }
        while (sprites[0].color.a > 0)
        {
            foreach (SpriteRenderer s in sprites)
            {
                Color c = s.color;
                c.a -= time;
                s.color = c;
            }
            yield return null;
        }
    }

}
