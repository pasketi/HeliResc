using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Copter : MonoBehaviour {

    protected GameManager gameManager;

    //Upgradable items
    public CargoSpace cargo;
    public Engine engine;
    public FuelTank fuelTank;
    public Rope rope;
    protected Dictionary<string, Upgradable> copterUpgrades;

    protected Rigidbody2D copterBody;

    protected CopterInput input;    //input class to control the copter

    protected float copterScale;

    void OnDestroy() {
        if(input != null)
            input.InputUpdate -= HandleInput;
    }

	// Use this for initialization
	protected virtual void Start () {
        Debug.Log("Copter start");
        cargo.Init(this);
        engine.Init(this);
        fuelTank.Init(this);
        rope.Init(this);
        input = new CopterInput();

        copterUpgrades = new Dictionary<string, Upgradable>();
        copterUpgrades.Add(cargo.name, cargo);
        copterUpgrades.Add(engine.name, engine);
        copterUpgrades.Add(fuelTank.name, fuelTank);
        copterUpgrades.Add(rope.name, rope);

        input.InputUpdate += HandleInput;
        input.IdleUpdate += IdleInput;

        copterBody = GetComponent<Rigidbody2D>();
        copterScale = transform.localScale.x;
	}    

    protected virtual void Update() {
        input.UpdateMethod();
        foreach (KeyValuePair<string, Upgradable> entry in copterUpgrades) {
            entry.Value.Update();
        }
    }    

    //Decides what to do with input
    protected virtual void HandleInput(Touch touch) {
        if (touch.phase.Equals(TouchPhase.Began)) {
            foreach (KeyValuePair<string, Upgradable> entry in copterUpgrades) {
                entry.Value.TapUpdate(touch);
            }
        }
        if (touch.phase.Equals(TouchPhase.Moved)) {
            foreach (KeyValuePair<string, Upgradable> entry in copterUpgrades) {
                entry.Value.InputUpdate(touch);
            }
        }
    }
    //What happens if there is no input
    protected virtual void IdleInput() {
        engine.Idle();
    }

    public void Direction(bool faceRight) {
        if (faceRight == true) {
            transform.localScale = new Vector3(copterScale, transform.localScale.y);
        } else {
            transform.localScale = new Vector3(-copterScale, transform.localScale.y);
        }
    }

    public virtual void BuyUpgrade(string upgrade) { }
}
