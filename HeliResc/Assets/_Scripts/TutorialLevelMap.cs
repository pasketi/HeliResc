﻿using UnityEngine;
using System.Collections;

public class TutorialLevelMap : MonoBehaviour {

    public GameObject finger;

	// Use this for initialization
	void Start () {
        finger.SetActive(false);
        StartCoroutine(Init());
    }

    private IEnumerator Init() {        
        LevelSetHandler set = GetComponent<LevelSetHandler>();
        bool passed = Level.Load(set.setName, 0).star1;
        //Debug.Log(set.setName + " " + passed);
        bool showFinger = set.Unlocked && !passed;
        if (showFinger == true && !set.Set.animated)
        {
            yield return new WaitForSeconds(2.5f);
            finger.SetActive(showFinger);
        }
        else
            finger.SetActive(showFinger);
    }
	
}
