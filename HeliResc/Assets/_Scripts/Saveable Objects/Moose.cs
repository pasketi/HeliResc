using UnityEngine;
using System.Collections;

public class Moose : HookableObject {

    public Sprite moose1;
    public Sprite moose2;
    public int faceChangeRate = 2;

    private SpriteRenderer sprite;
    private GameObject standingMoose;
    private GameObject hookedMoose;

    protected override void Start()
    {
        base.Start();        

        standingMoose = transform.Find("StandingMoose").gameObject;
        hookedMoose = transform.Find("HookedMoose").gameObject;

        sprite = standingMoose.GetComponent<SpriteRenderer>();

        hookedMoose.SetActive(false);
        standingMoose.SetActive(true);

        InvokeRepeating("FacialExpressions", faceChangeRate, faceChangeRate);
    }

    public override void GrabHook(Rigidbody2D hookRb)
    {
        base.GrabHook(hookRb);
        
        CancelInvoke();

        hookedMoose.SetActive(true);
        standingMoose.SetActive(false);
    }
    public override void DetachHook()
    {
        base.DetachHook();

        InvokeRepeating("FacialExpressions", faceChangeRate, faceChangeRate);

        hookedMoose.SetActive(false);
        standingMoose.SetActive(true);
    }

    private void FacialExpressions() {
        float f = Random.value;
        if (f > 0.5)
            StartCoroutine(ChangeSprite());
    }

    private IEnumerator ChangeSprite() {
        sprite.sprite = moose2;
        yield return new WaitForSeconds(1.5f);
        if(hooked == false)
            sprite.sprite = moose1;
    }
}
