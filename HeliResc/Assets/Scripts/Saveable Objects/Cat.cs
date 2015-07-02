using UnityEngine;
using System.Collections;

public class Cat : SaveableObject {

    public GameObject hookedCat;
    public GameObject nonHookedCat;
    

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
}
