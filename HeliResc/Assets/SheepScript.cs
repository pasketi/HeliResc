using UnityEngine;
using System.Collections;

public class SheepScript : ActionableObject
{

    public int group = 1;
    public float runSpeed;

    private float groundLevel;
    private bool saved;
    private bool runningToWater;
    private LevelManager manager;
    private Animator animator;

	// Use this for initialization
	void Start () {
        manager = GameObject.FindObjectOfType<LevelManager>();
        animator = GetComponent<Animator>();
        groundLevel = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void UseAction()
    {
        if (saved == true) return;
        saved = true;
        manager.saveCrates(1);
    }

    public void RunToWater(Transform water) {
        if (saved == true && runningToWater == false) return;
        runningToWater = true;
        animator.Play("Sheep_Thirsty_Running");
        StartCoroutine(RunToTarget(water.position));
        //StartCoroutine(Jump());
    }

    private void ReachWater() {
        runningToWater = false;
        animator.Play("Sheep_Happy_Idle");
        UseAction();
    }

    private IEnumerator Jump() {

        while (transform.position.y < groundLevel + 0.1f)
        {
            transform.Translate(Vector3.up * Time.deltaTime);
            yield return null;
        }
        while (transform.position.y > groundLevel) 
        {
            transform.Translate(-Vector3.up * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, groundLevel);
        if (runningToWater == true)
            StartCoroutine(Jump());
    }

    private IEnumerator RunToTarget(Vector3 target) {
        int direction = target.x > transform.position.x ? 1 : -1;
        float distance = Vector2.Distance(Vector2.right * target.x, Vector2.right * transform.position.x);
        float distWent = 0;

        while (distWent < distance) {
            float f = runSpeed * Time.deltaTime;
            distWent += f;
            transform.Translate(Vector3.right * direction * f);
            yield return null;
        }
        ReachWater();
    }
}
