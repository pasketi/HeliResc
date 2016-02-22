﻿using UnityEngine;
using System.Collections;

public class Diver : HookableObject {

    protected Animator animator;

    protected override void Start() {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void GrabHook(Rigidbody2D hookRb) {
        base.GrabHook(hookRb);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        animator.Play("Hooked");
    }

    public override void DetachHook() {
        base.DetachHook();

        animator.Play("Diver");
    }

	public override void CargoItem () {
		base.CargoItem();

		manager.isDiverInCargo = true;
	}
}