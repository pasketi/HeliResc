using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LandingPadManager : MonoBehaviour {

    public delegate void LandingEvent(GameObject go);
    public event LandingEvent enterPlatform = (GameObject platform) => { };
    public event LandingEvent exitPlatform = (GameObject platform) => { };

    public bool canWin = false, once = false;
    private MissionObjectives objectives;
    private LevelManager manager;

	private float cycle;

	//private BoxCollider2D trigger;
	private CargoManager cargo;
    private Copter copter;

	// Use this for initialization
	void Start () {
		copter = GameObject.Find ("Copter").GetComponent<Copter>();
        objectives = GameObject.FindObjectOfType<MissionObjectives>();
        manager = GameObject.FindObjectOfType<LevelManager>();
	}
		

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.transform.tag == "Crate") {
            HookableObject s = other.GetComponent<HookableObject>();
            if (s != null) {
                s.SaveItem();
            }
		}

		if (other.gameObject.transform.tag == "Copter") {
			
			EventManager.TriggerEvent(SaveStrings.eEnterPlatform);
            if (enterPlatform != null) enterPlatform(gameObject);

			if (canWin == true) { 
				bool win = objectives.ObjectiveOneCompleted();
				if (win == true && !once) {
                    manager.winLevel();
					once = true;
				}
			}            
                        

            //if (cargo.getCargoCrates() > 0) {
            //    cargo.emptyCargo();
            //    other.GetComponent<CopterManagerTouch>().resetPower();
            //}

            //if (other.GetComponent<CopterManagerTouch>().isHookDead == true) {
            //    other.GetComponent<CopterManagerTouch>().isHookDead = false;
            //}
		}
	}

	void OnTriggerStay2D(Collider2D other){        
        //if (other.gameObject.transform.tag == "Copter") {
        //    if(repair) {
        //        if (other.GetComponent<CopterManagerTouch>().getHealth() < other.GetComponent<CopterManagerTouch>().getMaxHealth()) {
        //            other.GetComponent<CopterManagerTouch>().changeHealth((float)other.GetComponent<CopterManagerTouch>().getHealSpeed()*Time.deltaTime);
        //        }
        //    }
        //    if(refill) {
        //        if (other.GetComponent<CopterManagerTouch>().getFuel() < other.GetComponent<CopterManagerTouch>().getMaxFuel()) {
        //            other.GetComponent<CopterManagerTouch>().changeFuel(other.GetComponent<CopterManagerTouch>().getReFuelSpeed()*Time.deltaTime);
        //        }
        //    }
        //}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.transform.tag == "Copter") {
            
            //Trigger the landing events            
            EventManager.TriggerEvent(SaveStrings.eExitPlatform);
            exitPlatform(gameObject);

			GameObject.Find ("HUD").GetComponent<UIManager> ().refill = false;
		}
	}
	
    //void saveAllChildren (GameObject hook) {
    //    if (hook.tag.Equals("Crate")) {
    //        copter.cargo.saveHookedCrate(hook.GetComponentInChildren<CrateManager>().crateMass);
    //        hook.tag = "KillMe";
    //    }
    //    foreach (Transform child in hook.transform) {
    //        if (child.tag == "Crate"){
    //            if (child.FindChild("LegHook") != null && child.FindChild("LegHook").childCount != 0)
    //                saveAllChildren (child.transform.FindChild("LegHook").gameObject);
    //            copter.cargo.saveHookedCrate(child.GetComponentInChildren<CrateManager>().crateMass);
    //            child.tag = "KillMe";
    //        }
    //    }
    //}

    public void ResetEvents()
    {
        enterPlatform = (GameObject platform) => { };
        exitPlatform = (GameObject platform) => { };
    }

    
	
	public void StartRefill() {
        if(copter == null)
            copter = GameObject.Find("Copter").GetComponent<Copter>();
        copter.fuelTank.FillTank();
		GameObject.Find ("HUD").GetComponent<UIManager> ().refill = true;
	}
}
