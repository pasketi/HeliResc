using UnityEngine;
using System.Collections;

public class Swimmer : ActionableObject {

    protected Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        //animator.SetBool("Saved", false);
    }    

    public override void UseAction() {
        animator.Play("SavedSwimmer");
    }
}
