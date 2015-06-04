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

        copterUpgrades = new Dictionary<string, Upgradable>();

        cargo.Init(this);
        engine.Init(this);
        fuelTank.Init(this);
        rope.Init(this);
        input = new CopterInput();
        
        input.InputUpdate += HandleInput;
        input.TouchStart += TouchStarted;
        input.TouchEnd += TouchEnded;
        input.IdleUpdate += IdleInput;

        copterBody = GetComponent<Rigidbody2D>();
        copterScale = transform.localScale.x;
	}

    public void AddToDictionary(Upgradable u) {
        copterUpgrades.Add(u.name, u);
    }

    protected virtual void Update() {
        input.UpdateMethod();
        foreach (Upgradable entry in copterUpgrades.Values) {
            entry.Update();
        }
    }    

    //Decides what to do with input
    protected virtual void HandleInput(MouseTouch touch) {        
        foreach (Upgradable entry in copterUpgrades.Values) {
            entry.InputUpdate(touch);
        }
    }
    protected void TouchStarted(MouseTouch touch) {
        foreach (Upgradable entry in copterUpgrades.Values) {
            entry.TouchStart(touch);
        }
    }
    protected void TouchEnded(MouseTouch touch) {
        foreach (Upgradable entry in copterUpgrades.Values) {
            entry.TouchEnd(touch);
        }
    }
    //What happens if there is no input
    protected virtual void IdleInput() {
        engine.IdleInput();
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
