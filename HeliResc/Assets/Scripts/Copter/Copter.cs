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
    public string copterName;
	public int price;
	public string description;
	public bool unlocked;
	public int requiredStars;
	public int requiredRubies;

    //Upgradable items
    public CargoSpace cargo;
    public Engine engine;
    public FuelTank fuelTank;
    public Rope rope;
    public CopterHealth health;
    protected Dictionary<string, Upgradable> copterUpgrades;
    public Dictionary<string, Upgradable> Upgrades { get { return copterUpgrades; } set { } }

    public HookScript HookScript { get { return rope.HookScript; } }

    protected Rigidbody2D copterBody;

    protected CopterInput input;    //input class to control the copter

    protected bool onPlatform;
    public bool OnPlatform { get { return onPlatform; } }
    protected float copterScale;

    void OnDestroy() {
        if(input != null)
            input.InputUpdate -= HandleInput;
    }

    protected virtual void RegisterListeners() {        
        foreach (Upgradable entry in copterUpgrades.Values) {
            entry.RegisterListeners();
        }
        EventManager.StartListening(SaveStrings.eCopterSplash, TurnCopterOff);
        EventManager.StartListening(SaveStrings.eEnterPlatform, EnterPlatform);
        EventManager.StartListening(SaveStrings.eExitPlatform, ExitPlatform);
    }
    protected virtual void OnDisable() {
        foreach (Upgradable entry in copterUpgrades.Values) {
            entry.UnregisterListeners();
        }
        EventManager.StopListening(SaveStrings.eCopterSplash, TurnCopterOff);
        EventManager.StopListening(SaveStrings.eEnterPlatform, EnterPlatform);
        EventManager.StopListening(SaveStrings.eExitPlatform, ExitPlatform);
    }

	// Use this for initialization
	protected virtual void Awake () {
        
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
		unlocked = PlayerPrefsExt.GetBool (copterName + "Unlocked") || unlocked;


        UpdateMethod = NormalUpdate;
        
        copterSprite = GetComponent<SpriteRenderer>().sprite;

        copterUpgrades = new Dictionary<string, Upgradable>();

        AddUpgradables();
        input = new CopterInput();

        RegisterListeners();

        input.InputUpdate += HandleInput;
        input.TouchStart += TouchStarted;
        input.TouchEnd += TouchEnded;
        input.IdleUpdate += IdleInput;

        copterBody = GetComponent<Rigidbody2D>();
        copterScale = transform.localScale.x;
		SaveCopter ();
	}
    protected virtual void Update() {
        UpdateMethod();
    }
    protected virtual void AddUpgradables() {
        cargo.Init(this);
        engine.Init(this);
        fuelTank.Init(this);
        rope.Init(this);
        health.Init(this);
    }
    public void AddToDictionary(Upgradable u) {
        copterUpgrades.Add(u.Name, u);
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
    protected virtual void EnterPlatform() {
        onPlatform = true;
    }
    protected virtual void ExitPlatform() {
        onPlatform = false;
    }
    public void Direction(bool faceRight) {
        if (faceRight == true) {
            transform.localScale = new Vector3(copterScale, transform.localScale.y);
        } else {
            transform.localScale = new Vector3(-copterScale, transform.localScale.y);
        }
    }

    public virtual void Detonate() {
        health.Detonate();
    }
    public virtual void TurnCopterOff() {
        UpdateMethod = TurnedOffUpdate;
    }
    public virtual void Kinematic(bool p) {
        copterBody.isKinematic = p;
        fuelTank.Kinematic(p);
        if (p == true)
            input.DisableInput();
        else
            input.EnableInput();
    }
    public virtual void Disable() {
        gameObject.SetActive(false);
    }
    public virtual void UseAction() { }
	
    public virtual void SaveCopter() {
		PlayerPrefsExt.SetBool (copterName + "Unlocked", unlocked);
	}
    public virtual Copter LoadCopter() { return this; }

    public virtual GameObject CreateGameObject(GameObject prefab, Vector3 position, Quaternion rotation) {
        return (Instantiate(prefab, position, rotation) as GameObject);
    }
	public virtual CopterInfo GetCopterInfo(int index) {
		CopterInfo info = new CopterInfo ();

		int pStars = PlayerPrefs.GetInt (SaveStrings.sPlayerStars, 0);
		int pRuby = PlayerPrefs.GetInt (SaveStrings.sPlayerRubies, 0);

		if (requiredStars != 0) {
			info.buyable = (requiredStars <= pStars);
		} else if (requiredRubies != 0) {
			info.buyable = (requiredRubies <= pRuby);
		} else
			info.buyable = true;

		info.unlocked = unlocked;
		info.copterIndex = index;
		info.copterSprite = GetComponent<SpriteRenderer>().sprite;
		info.copterName = copterName;
		info.copterPrice = price;
		info.fuelAmount = fuelTank.maxCapacity;
		info.enginePower = engine.maxPower;
		info.cargoSpace = cargo.maxCapacity;
		info.copterColor = GetComponent<SpriteRenderer> ().color;
		info.description = description;
		info.requiredStars = requiredStars;
		info.requiredRubies = requiredRubies;

		return info;
	}
}

public class CopterInfo {

	public void Save() {
		PlayerPrefsExt.SetBool (copterName + "Unlocked", unlocked);
	}
	public void Load() {
		int pStars = PlayerPrefs.GetInt (SaveStrings.sPlayerStars, 0);
		int pRuby = PlayerPrefs.GetInt (SaveStrings.sPlayerRubies, 0);
		if (requiredStars != 0) {
			buyable = (requiredStars <= pStars);
		} else if (requiredRubies != 0) {
			buyable = (requiredRubies <= pRuby);
		} else
			buyable = true;

		unlocked = PlayerPrefsExt.GetBool (copterName + "Unlocked");
	}

	public string description;
	public int cargoSpace;
	public float enginePower;
	public float fuelAmount;
	public Sprite copterSprite;
	public string copterName;
	public int copterIndex;
	public int copterPrice;
	public int requiredStars;
	public int requiredRubies;
	public bool unlocked;
	public bool buyable;
	public Color copterColor;
}
