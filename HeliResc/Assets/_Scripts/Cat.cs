using UnityEngine;
using System.Collections;

public class Cat : HookableObject {

    public GameObject hookedCat;
    public GameObject nonHookedCat;
    
	protected override void Update () {
		base.Update();

		if (manager.isCatDry != false && !hooked) {
			manager.isCatDry = !floating.IsInWater;
		}
	}

    public override void GrabHook(Rigidbody2D hookRb) {
        base.GrabHook(hookRb);

        hookedCat.SetActive(true);
        nonHookedCat.SetActive(false);
    }

    public override void DetachHook() {
        base.DetachHook();

        hookedCat.SetActive(false);
        nonHookedCat.SetActive(true);
    }

    public override void CargoItem() {
        base.CargoItem();
    }

	protected virtual void OnCollisionEnter2D(Collision2D collision) {
		base.OnCollisionEnter2D(collision);

		if (collision.gameObject.CompareTag("CatIsland")) manager.isCatDry = false;
	}
}
