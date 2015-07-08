using UnityEngine;
using System.Collections;

public class Diver : HookableObject {

    protected Animator animator;

    protected override void Start() {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void GrabHook(Rigidbody2D hookRb) {
        base.GrabHook(hookRb);

        animator.Play("Hooked");
    }

    public override void DetachHook() {
        base.DetachHook();

        animator.Play("Diver");
    }
}