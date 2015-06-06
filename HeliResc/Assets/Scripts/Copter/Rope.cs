using UnityEngine;
using System.Collections;

[System.Serializable]
public class Rope : Upgradable {

    public int durability;
    public float reelSpeed;             //Value of how fast the hook gets back to copter

    //Hook variables
    public GameObject hook;             //The hook object to throw out
    public GameObject hookAnchor;       //Anchor of the hook. Needs to be set in inspector
    public float hookDistance = 1.5f;   //The distance of the hook from the anchor point
    
    private DistanceJoint2D hookJoint;  //DistanceJoint component of the copter
    private bool hookOut;               //Is the hook out or inside the copter
    private bool hasHook;               //Determines if the copter has a hook or is it destroyed

    public override void Init(Copter copter) {
        base.Init(copter);

        UpdateDelegate = HookInUpdate;                          //Start with the hook in the copter

        hasHook = true;
        hookJoint = playerRb.GetComponent<DistanceJoint2D>();
        hookJoint.anchor = hookAnchor.transform.localPosition;
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public override void TouchStart(MouseTouch touch) {
        if (playerRb.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.position)) && hasHook) {
            hookOut = !hookOut;
            if (hookOut == true) UpdateDelegate = HookOutUpdate;    //Change the update method to a correct one
            else UpdateDelegate = HookInUpdate;
        }
    }

    public void HookInUpdate() {
        
        if (hook != null && hasHook)
        {
            hookJoint.distance -= reelSpeed * Time.deltaTime;
        }        
    }
    public void HookOutUpdate() {
        if (hookOut && hook == null && hasHook)
        {
            hook.transform.position = playerRb.transform.position + new Vector3(0f, -0.3f);
            hook.transform.rotation = Quaternion.identity;
            hookJoint.enabled = true;
            hookJoint.distance = hookDistance;
            hookJoint.connectedBody = hook.GetComponent<Rigidbody2D>();
        }
        else if (hook != null && !hookOut && Vector2.Distance(hook.transform.position, hookAnchor.transform.position) < 0.1 && hasHook)
        {
            playerCopter.cargo.CargoHookedCrates(hook);
            if (playerCopter.cargo.CurrentCargo >= playerCopter.cargo.maxCapacity && hook.transform.childCount > 0)
            {
                hookOut = true;
                Debug.Log("Cargo full");
            }
            else if (hook.transform.childCount == 0)
            {
                hook.SetActive(false);
                //Destroy(hook);
                //once = false;
            }
        }
        else if (hookOut && hookJoint.distance != hookDistance)
        {
            hookJoint.distance = hookDistance;
        }
        if (hasHook && hookOut && Vector2.Distance(hook.transform.position, hookAnchor.transform.position) > hookDistance * 2)
        {
            //killHook();
        }
    }
    public void HookDestroyedUpdate() { 
    
    }
}
