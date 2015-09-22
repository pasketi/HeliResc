using UnityEngine;
using System.Collections;

public class ForestFloor : MonoBehaviour {

    public GameObject puddlePrefab;	

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Pond")) {
            CreatePuddle(other.transform.position);
            other.gameObject.SetActive(false);
        }
    }

    private void CreatePuddle(Vector3 position) {
        GameObject go = Instantiate(puddlePrefab) as GameObject;
        Vector3 pos = new Vector3(position.x, transform.position.y);
        go.transform.position = pos;
    }
}
