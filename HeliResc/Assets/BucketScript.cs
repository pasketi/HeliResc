using UnityEngine;
using System.Collections;

public class BucketScript : HookScript {

    public Sprite fullSprite;
    public GameObject waterPrefab;      //Prefab of the water that can be thrown

    private Animator animator;          //Reference to animator component
    private SpriteRenderer sprite;      //Reference to sprite renderer component

    private bool full;                  //Is the bucket full or empty
    private bool canThrowWater;         //Player should not be able to throw continuously to avoid glitches with animator

    private GameObject waterDrop;

    protected override void Start() {
        base.Start();

        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        waterDrop = Instantiate(waterPrefab) as GameObject;
        waterDrop.SetActive(false);
        canThrowWater = true;
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Pond") == true) {
            Fill();
        }
    }

    public void Fill() {
        full = true;
        Debug.Log("Fill");
        animator.Play("IdleFull");
    }

    public void Throw() {
        Debug.Log("Throw water: " + canThrowWater);        
        OpenBucket();
        DropWater();
    }

    public void OpenBucket() {
        if (canThrowWater == false) return;

        canThrowWater = false;
        if (full == true)
            animator.Play("OpenFull");
        else
            animator.Play("OpenEmpty");
        full = false;
        StartCoroutine(BucketCooldown());
    }

    private void DropWater() {
        waterDrop.SetActive(true);
        waterDrop.transform.position = transform.position - (Vector3.up * 0.75f);
    }

    private IEnumerator BucketCooldown() {
        yield return new WaitForSeconds(2);
        canThrowWater = true;
    }
}