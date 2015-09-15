using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireworksController : MonoBehaviour {

    public Fireworks[] fireworks;
    public float range = 2f;
    public float timeToWait = 1;

    private List<ParticleSystem> particles;
    private bool launched;

    void Start() {
        particles = new List<ParticleSystem>();

        foreach (Fireworks fw in fireworks) {
            for (int i = 0; i < fw.amount; i++) {
                GameObject go = Instantiate(fw.prefab) as GameObject;
                particles.Add(go.GetComponent<ParticleSystem>());

            }
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            Launch();
        }
    }

    private ParticleSystem PopFromList() {
        if (particles.Count > 0) {
            ParticleSystem p = particles[Random.Range(0, particles.Count - 1)];
            particles.Remove(p);
            return p;
        }
        return null;
    }

    public void Launch() {
        if (launched == false)
        {
            launched = true;
            StartCoroutine(Launcher());
        }
    }

    public IEnumerator Launcher() {
        Vector3 vec = Camera.main.ScreenToWorldPoint(0.5f * (new Vector3(Screen.width, Screen.height)));
        ParticleSystem ps = PopFromList();
        float rangeX = range * Screen.width / Screen.height;
        float rangeY = range * Screen.height / Screen.width;
        while(ps != null) {
                        
            float x = Random.Range(vec.x - rangeX, vec.x + rangeX);
            
            float y = Random.Range(vec.y - rangeY, vec.y + rangeY) + 3;
            ps.transform.position = new Vector3(x, y);
            
            ps.Play();
            yield return new WaitForSeconds(timeToWait);
            ps = PopFromList();
        }
    }
}
[System.Serializable]
public class Fireworks {
    public GameObject prefab;
    public int amount;
}
