using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class flashOfWhiteManager : MonoBehaviour {

	public GameObject levelSet;
	public bool trigger = false;
	private LevelSetHandler handler;

	void Update () {
		if (trigger) {
			complete ();
			trigger = false;
		}
	}

	public bool setLevelSet (GameObject levelS) {
		levelSet = levelS;
		handler = levelSet.GetComponent<LevelSetHandler>();
		return levelSet != null ? true : false ;
	}

	public void playAnimation () {
		gameObject.GetComponent<Animator>().Play("flash");
		gameObject.GetComponent<Animator>().SetTrigger("end");
	}

	public void complete () {
		handler.setChest.enabled = false;
		handler.setBG.sprite = handler.setBGCompleted;
	}

}
