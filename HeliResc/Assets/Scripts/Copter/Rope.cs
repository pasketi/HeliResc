using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Rope : Upgradable {

    public int durability;
    public float reelSpeed;             //Value of how fast the hook gets back to copter

    //Hook variables
    private GameObject hook;            //The hook object to throw out
    public GameObject hookPrefab;       //The prefab to Instantiate
    public GameObject hookAnchor;       //Anchor of the hook. Needs to be set in inspector
    public float hookDistance = 1.5f;   //The distance of the hook from the anchor point
    public float snapDistance = 3;      //The distance the rope will snap
    public float hookMass;              //The mass to assign to the rigidbody
    
    private DistanceJoint2D hookJoint;  //DistanceJoint component of the copter
    private bool hookOut;               //Is the hook out or inside the copter
    private bool hasHook;               //Determines if the copter has a hook or is it destroyed    

    private HookScript hookScript;      //The reference to the hook script of the hook
    public HookScript HookScript { get { return hookScript; } }

    public bool HasHook { get { return hasHook; } }


    public override void RegisterListeners() {
        EventManager.StartListening("CopterExplode", KillHook);
        EventManager.StartListening("EnterPlatform", RestoreHook);
    }
    public override void UnregisterListeners() {
        EventManager.StopListening("CopterExplode", KillHook);
        EventManager.StopListening("EnterPlatform", RestoreHook);
    }

    public override void Init(Copter copter) {
        base.Init(copter);
        
        //hook = playerCopter.CreateGameObject(hookPrefab, Vector3.zero, Quaternion.identity);

        CreateHook(hookPrefab, hookDistance, snapDistance);
    }
    
    public virtual void CreateHook(GameObject newHook, float distance, float snapDistance) {
        if(hook != null) {
            hook.SetActive(false);
            hook = null;
        }

		//Destroy old hook if there is any
		if(hook != null) GameObject.Destroy (hook);
        
		hookPrefab = newHook;
        hook = playerCopter.CreateGameObject(hookPrefab, Vector3.zero, Quaternion.identity);
        hookScript = hook.GetComponent<HookScript>();
        hookScript.HookMass = hookMass;

        hasHook = true;
        hookJoint = playerRb.GetComponent<DistanceJoint2D>();
        hookJoint.anchor = hookAnchor.transform.localPosition;

        hookDistance = distance;
        this.snapDistance = snapDistance;

        PushHookToCargo(true);
        
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public override void TouchStart(MouseTouch touch) {        
        if (playerRb.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.position)) && hasHook) {
            hookOut = !hookOut;

            if (hookOut == true) {              //Actions to take when hook is thrown                
                ThrowHook();
            } else {                            //Actions to take when hook is summoned back in                    
                UpdateDelegate = ReelInUpdate; 
            }            
        }
    }

    public void ReelInUpdate() {        
        if (hasHook == true) {
            
            hookJoint.distance -= reelSpeed * Time.deltaTime;
            if (hookJoint.distance <= reelSpeed * Time.deltaTime) {                                
                PushHookToCargo();
            }
        }        
    }
    public void HookOutUpdate() {                       
        if (hasHook && hookOut && Vector2.Distance(hook.transform.position, hookAnchor.transform.position) > snapDistance) {
            KillHook();
        }
    }
    public void HookDestroyedUpdate() { 
    
    }

    private void PushHookToCargo(bool forcePush = false) {

        List<HookableObject> hookedObject = new List<HookableObject>();
        if (forcePush == false) {
            hookedObject = hookScript.HookedItems;
        }
        
        hookedObject = playerCopter.cargo.CargoHookedCrates(hookedObject);

        hookScript.ResetHook(hookedObject);

        if (hookedObject.Count > 0) {
            hookOut = true;
            ThrowHook();
        } else {
            hookJoint.enabled = false;
            hookJoint.distance = 0;

            UpdateDelegate = () => { };
            hook.SetActive(false);
        }
         
        /*Debug.Log("Child count: " + hook.transform.childCount);
        while (hook.transform.childCount > 0 && playerCopter.cargo.CargoFull == false) {
            
            Debug.Log("foreach loop");
            playerCopter.cargo.CargoHookedCrates(hook.transform.GetChild(0));
        }
        Debug.Log("Cargo hooked crates");
        if (playerCopter.cargo.CargoFull && hook.transform.childCount > 0 && forcePush == false) {
            hookOut = true;
            ThrowHook();
            UpdateDelegate = HookOutUpdate;

        } else {
            hookJoint.enabled = false;
            hookJoint.distance = 0;

            UpdateDelegate = () => { };
            hook.SetActive(false);
        }*/
    }

    private void ThrowHook() {
        
        hook.SetActive(true);
        hook.GetComponent<LineRenderer>().enabled = true;
        hookJoint.enabled = true;
        hookJoint.connectedBody = hook.GetComponent<Rigidbody2D>();
        hookJoint.distance = hookDistance;
        hook.transform.position = hookAnchor.transform.position;
        UpdateDelegate = HookOutUpdate;
    }
    public void KillHook() {                
        hook.GetComponent<LineRenderer>().enabled = false;
        EventManager.TriggerEvent("HookDied");
        hasHook = false;
        hookJoint.enabled = false;              
    }
    protected virtual void RestoreHook() {
        if (hasHook == true) return;
        hasHook = true;
        PushHookToCargo(true);
    }
    protected override void GiveName() {
        name = "Rope";
    }
}
