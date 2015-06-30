using UnityEngine;
using System.Collections;

public class Cat : SaveableObject {

    public GameObject hookedCat;
    public GameObject nonHookedCat;
    

    public override void GrabHook() {
        base.GrabHook();

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
