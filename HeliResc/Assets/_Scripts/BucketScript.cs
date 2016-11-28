using UnityEngine;
using System.Collections;

public class BucketScript : HookScript {

    public Sprite fullSprite;
    public GameObject waterPrefab;          //Prefab of the water that can be thrown

    private Animator _animator;             //Reference to animator component
    private SpriteRenderer _sprite;         //Reference to sprite renderer component

    private bool full;                      //Is the bucket full or empty
    private bool canThrowWater;             //Player should not be able to throw continuously to avoid glitches with animator

    private GameObject waterDrop;
    private ParticleSystem waterParticles;
	private Rigidbody2D waterRb;
    private Rigidbody2D _rigidbody;

    protected override void Start() {
        base.Start();

        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
		_rigidbody = GetComponent<Rigidbody2D> ();

        waterDrop = Instantiate(waterPrefab) as GameObject;
		waterRb = waterDrop.GetComponent<Rigidbody2D> ();
        waterParticles = waterDrop.GetComponent<ParticleSystem>();

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
        //Debug.Log("Fill");
        _animator.Play("IdleFull");
    }

    public void Throw() {
        //Debug.Log("Throw water: " + canThrowWater);        
        OpenBucket();
    }

    public void OpenBucket() {
        if (canThrowWater == false) return;

        canThrowWater = false;
        if (full == true) {
			_animator.Play ("OpenFull");
			//DropWater ();
		}
        else
            _animator.Play("OpenEmpty");
        full = false;
        StartCoroutine(BucketCooldown());
    }

    private void DropWater() {
        waterDrop.SetActive(true);
        waterParticles.Play();
		waterRb.velocity = _rigidbody.velocity;
		waterDrop.transform.position = transform.position - (transform.up * .5f);
        waterDrop.transform.eulerAngles = transform.eulerAngles;
    }

    private IEnumerator BucketCooldown() {
        yield return new WaitForSeconds(2);
        canThrowWater = true;
    }
}