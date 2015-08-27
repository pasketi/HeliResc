using UnityEngine;
using System.Collections;

public class CrateManager : MonoBehaviour {

	private LevelManager manager;
	private Copter copterScript;
	
	private float floatyValue, maxLifeTimeInSeconds/*, flashTime = 0.25f, tempFlash = 0f*/; //buoancy?
	private bool 	lastInWater = false,
					stationary = false,
					twoPhases = false,
					fourPhases = false,
					flashWhite = false,
					onGround = true;
	private GameObject crate;
	private SpriteRenderer spriteRenderer, bgRenderer;
	private DistanceJoint2D joint;
	private HingeJoint2D hinge;
	private Animator animator;
	public float crateMass = 5f, inWaterModifier = 0.2f, lifeTimeInSeconds = 60f;


	/*public Sprite Phase100, 	//MANDATORY, First sprite
					Phase66,	//four phase, max 2/3s left
					Phase33, 	//four phase, max 1/3s left
					Phase0,		//two phase, dead
					Hooked, 	//MANDATORY
					Phase100BG,
					Phase66BG,
					Phase33BG,
					Phase0BG, 
					HookedBG;*/
	public bool 	inWater = false, 
					inCargo = false, 
					inMenu = false, 
					dying = false, 
					dead = false;



	// Use this for initialization
	void Start () {
		maxLifeTimeInSeconds = lifeTimeInSeconds;
		if (!inMenu) {
			Time.timeScale = 1f;
			manager = GameObject.Find ("LevelManagerO").GetComponent<LevelManager> ();
			copterScript = GameObject.FindObjectOfType<Copter> ();			
		}
		crate = gameObject.transform.parent.gameObject;
		animator = gameObject.transform.parent.GetComponent<Animator>();

		//spriteRenderer = crate.GetComponent<SpriteRenderer> ();
		/*if (crate.transform.FindChild ("BackGround") != null)
			bgRenderer = crate.transform.FindChild ("BackGround").GetComponent<SpriteRenderer> ();*/

		if (animator.GetInteger("status") == null){
			stationary = true;
		} else if (animator.GetInteger("status") == 1){
			twoPhases = true;
		} else if (animator.GetInteger("status") == 3){
			fourPhases = true;
		}
		
		floatyValue = gameObject.transform.parent.GetComponent<Rigidbody2D> ().mass * 30f;
		hinge = GetComponent<HingeJoint2D> ();
	}

	void FixedUpdate(){
		if (crate.CompareTag("KillMe")) Destroy(crate);
	}

	// Update is called once per frame
	void Update () {

		if (tag == "ActionableObject" && crate.layer == 11) {
			manager.saveCrates(1);
			tag = "SavedObject";
		}

		if (!inMenu) {

			if (dying && crate.layer != 11) {
				lifeTimeInSeconds -= Time.deltaTime;
				if (animator.GetFloat("lifeTime") != null) {
					animator.SetFloat("lifeTime", lifeTimeInSeconds);
					if (lifeTimeInSeconds <= 0f) {
						lifeTimeInSeconds = 0f;
						dead = true;
					}
				}
			}

			if (dying && twoPhases) {
				if (lifeTimeInSeconds <= 0f) {
					animator.SetInteger("status", 0);
				} else {
					animator.SetInteger("status", 1);
				}
			} else if (dying && fourPhases) {
				if (lifeTimeInSeconds <= 0f) {
                    Transform t = transform.parent.Find("iceskull");
                    if (t != null)
                        t.GetComponent<SpriteRenderer>().enabled = true;
					animator.SetInteger("status", 0);
				} else if (lifeTimeInSeconds <= (maxLifeTimeInSeconds/3)) {
					animator.SetInteger("status", 1);
				} else if (lifeTimeInSeconds <= (maxLifeTimeInSeconds/3)*2) {
					animator.SetInteger("status", 2);
				} else {
					animator.SetInteger("status", 3);                    
				}
			}

			/*if (dying) {
				if (lifeTimeInSeconds < 10f) {
					tempFlash += Time.deltaTime;
					if (tempFlash >= flashTime) {
						flashWhite = !flashWhite;
						tempFlash = 0f;
					}
				}

				if (flashWhite) bgRenderer.color = new Color(1f, 1f, 1f);
				else {
				if ((lifeTimeInSeconds / maxLifeTimeInSeconds) > 0.5f) bgRenderer.color = new Color(1f, 1f, (lifeTimeInSeconds - (maxLifeTimeInSeconds / 2)) / (maxLifeTimeInSeconds - (maxLifeTimeInSeconds / 2)));
				else if ((lifeTimeInSeconds / maxLifeTimeInSeconds) <= 0.5f) bgRenderer.color = new Color(1f, lifeTimeInSeconds / (maxLifeTimeInSeconds - (maxLifeTimeInSeconds / 2)), 0f);	
				}
			}*/

            if (copterScript == null || (!inCargo && copterScript != null && crate.layer == 11)) {
				Destroy (joint);
				//copterScript.cargo.ChangeHookMass (-crateMass);
				gameObject.GetComponent<Collider2D> ().enabled = true;
				crate.layer = 10;
				gameObject.transform.parent.parent = null;
			}

			if (crate != null && (!inCargo && crate.transform.position.y < manager.getWaterLevel ())) {
				if (manager.getPaused () == false && !dead)
					crate.GetComponent<Rigidbody2D> ().AddForce (Vector3.up * floatyValue);
				inWater = true;
				onGround = false;
				if (animator.GetBool("inWater") != null) animator.SetBool("inWater", true);
			} else {
				inWater = false;

			}

			if (animator.GetBool("inWater") != null && onGround == false) animator.SetBool("inWater", true);
			else if (animator.GetBool("inWater") != null && onGround == true) animator.SetBool("inWater", false);

			if (crate != null && copterScript != null && crate.layer == 11 && inWater != lastInWater) {
                if (twoPhases == true) return;
				lastInWater = inWater;
				if (inWater && !inCargo) {
					//copterScript.cargo.ChangeHookMass ((-crateMass) + (crateMass * inWaterModifier));
				} else if (!inWater || inCargo) {
					//copterScript.cargo.ChangeHookMass ((crateMass) + -(crateMass * inWaterModifier));
				}
			}

			if (crate.layer == 11) {
				/*spriteRenderer.sprite = Hooked;
				if (bgRenderer != null)
					bgRenderer.sprite = HookedBG;*/
				if (animator.GetBool ("otherHooked") != null) {
					if (transform.parent.FindChild("LegHook") != null && transform.parent.FindChild("LegHook").childCount > 0)
						animator.SetBool ("otherHooked", true);
					else
						animator.SetBool ("otherHooked", false);
				}
				animator.SetBool ("isHooked", true);
			} else {
				/*spriteRenderer.sprite = Phase100;
				if (bgRenderer != null)
					bgRenderer.sprite = Phase100BG;*/
				animator.SetBool ("isHooked", false);
				if (animator.GetBool ("otherHooked") != false && animator.GetBool ("otherHooked") != true)
					animator.SetBool ("otherHooked", false);
			}
		} else {
			if (crate.transform.position.y < 0f) {
				crate.GetComponent<Rigidbody2D> ().AddForce (Vector3.up * floatyValue);
				inWater = true;
			} else {
				inWater = false;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (!inMenu && !dead) {
			if (copterScript != null && collision.collider.gameObject.CompareTag ("Hook") && tag != "ActionableObject" && tag != "SavedObject") {
				gameObject.transform.parent.parent = collision.collider.gameObject.transform;
				gameObject.AddComponent <DistanceJoint2D> ();

				joint = GetComponent<DistanceJoint2D> ();


				joint.connectedBody = collision.collider.gameObject.GetComponent<Rigidbody2D> ();
				if (joint.connectedBody.gameObject.name == "Hook(Clone)")
					joint.connectedAnchor = new Vector2 (0f, -0.3f);
				else joint.connectedAnchor = new Vector2 (0f, 0f);
				joint.distance = 0f;
				joint.maxDistanceOnly = false;
				hinge.useLimits = false;

				gameObject.GetComponent<Collider2D> ().enabled = false;
				crate.layer = 11; //liftedCrate
				//copterScript.cargo.ChangeHookMass (crateMass);
			} else onGround = true;
		}
	}
}
