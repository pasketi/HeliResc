using UnityEngine;
using System.Collections;

public class Crate : HookableObject {

    public Sprite[] nonHookedSprites;   //assign the crate sprite to the first cell and bg to the second
    public Sprite[] hookedSprites;      //Assign in inspector the hooked sprites. Crate sprite to the first and bg to the second
    public SpriteRenderer crate;
    public SpriteRenderer background;

    public override void GrabHook(Rigidbody2D hookRb) {
        base.GrabHook(hookRb);

		if (hookScript.HookedItems.Count >= 2) manager.multipleCratesHooked = true;

		crate.gameObject.layer = LayerMask.NameToLayer("liftedCrateCollider");

        crate.sprite = hookedSprites[0];
        background.sprite = hookedSprites[1];
    }

    public override void DetachHook() {
        base.DetachHook();

		crate.gameObject.layer = LayerMask.NameToLayer("CrateCollider");

        crate.sprite = nonHookedSprites[0];
        background.sprite = nonHookedSprites[1];
    }

    public override void CargoItem() {
        base.CargoItem();
    }

}