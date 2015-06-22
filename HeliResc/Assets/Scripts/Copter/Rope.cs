using UnityEngine;
using System.Collections;

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
    
    private DistanceJoint2D hookJoint;  //DistanceJoint component of the copter
    private bool hookOut;               //Is the hook out or inside the copter
    private bool hasHook;               //Determines if the copter has a hook or is it destroyed    

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
        
        hook = playerCopter.CreateGameObject(hookPrefab, Vector3.zero, Quaternion.identity);

        hasHook = true;
        hookJoint = playerRb.GetComponent<DistanceJoint2D>();
        hookJoint.anchor = hookAnchor.transform.localPosition;

        PushHookToCargo();
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

    private void PushHookToCargo() {

        playerCopter.cargo.CargoHookedCrates(hook);
        if (playerCopter.cargo.CargoFull && hook.transform.childCount > 0) {
            hookOut = true;
            ThrowHook();
            UpdateDelegate = HookOutUpdate;

        } else { 
            hookJoint.enabled = false;
            hookJoint.distance = 0;

            UpdateDelegate = () => { };
            hook.SetActive(false);
        }
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
        if (hookOut == false) return;
        hasHook = false;
        hookJoint.enabled = false;
        hook.GetComponent<LineRenderer>().enabled = false;        
    }
    protected virtual void RestoreHook() {
        hasHook = true;
    }
    protected override void GiveName() {
        name = "Rope";
    }
}
