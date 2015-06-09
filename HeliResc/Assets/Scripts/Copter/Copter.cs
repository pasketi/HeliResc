using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Copter : MonoBehaviour {

    public GameManager gameManager;
    public LevelManager levelManager;

    protected Action UpdateMethod = () => { };

    //Copter specific variables
    public Sprite copterSprite;
    public string name;

    //Upgradable items
    public CargoSpace cargo;
    public Engine engine;
    public FuelTank fuelTank;
    public Rope rope;
    public CopterHealth health;
    protected Dictionary<string, Upgradable> copterUpgrades;
    public Dictionary<string, Upgradable> Upgrades { get { return copterUpgrades; } set { } }

    protected Rigidbody2D copterBody;

    protected CopterInput input;    //input class to control the copter

    protected float copterScale;

    void OnDestroy() {
        if(input != null)
            input.InputUpdate -= HandleInput;
    }

    protected virtual void RegisterListeners() {        
        foreach (Upgradable entry in copterUpgrades.Values) {
            entry.RegisterListeners();
        }
        EventManager.StartListening("CopterSplash", TurnCopterOff);
    }
    protected virtual void OnDisable() {
        foreach (Upgradable entry in copterUpgrades.Values) {
            entry.UnregisterListeners();
        }
        EventManager.StopListening("CopterSplash", TurnCopterOff);
    }

	// Use this for initialization
	protected virtual void Awake () {
        
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        gameManager = GameObject.FindObjectOfType<GameManager>();

        UpdateMethod = NormalUpdate;

        copterSprite = GetComponent<SpriteRenderer>().sprite;

        copterUpgrades = new Dictionary<string, Upgradable>();

        cargo.Init(this);
        engine.Init(this);
        fuelTank.Init(this);
        rope.Init(this);
        health.Init(this);
        input = new CopterInput();

        RegisterListeners();

        input.InputUpdate += HandleInput;
        input.TouchStart += TouchStarted;
        input.TouchEnd += TouchEnded;
        input.IdleUpdate += IdleInput;

        copterBody = GetComponent<Rigidbody2D>();
        copterScale = transform.localScale.x;
	}
    protected virtual void Update() {
        UpdateMethod();
    }

    public void AddToDictionary(Upgradable u) {
        copterUpgrades.Add(u.name, u);
    }
    public void SetInputActive(bool active) {
        if (active == true) input.EnableInput();
        else input.DisableInput();
    }
    protected virtual void NormalUpdate() {
        input.UpdateMethod();
        foreach (Upgradable entry in copterUpgrades.Values) {
            entry.Update();
        }
    }
    protected virtual void TurnedOffUpdate() { /*Update to run when copter is gone under water or turned off Add something here if needed.*/ }

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
    public virtual void TurnCopterOff() {
        UpdateMethod = TurnedOffUpdate;
    }
    public virtual void Disable() {
        gameObject.SetActive(false);
    }
    public virtual void UseAction() { }

    public virtual void BuyUpgrade(string upgrade) { }
    public virtual void SaveCopter() { }
    public virtual Copter LoadCopter() { return this; }

    public virtual GameObject CreateGameObject(GameObject prefab, Vector3 position, Quaternion rotation) {
        return (Instantiate(prefab, position, rotation) as GameObject);
    }    
}
