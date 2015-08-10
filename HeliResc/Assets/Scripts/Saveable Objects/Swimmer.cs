using UnityEngine;
using System.Collections;

public class Swimmer : ActionableObject {

	protected bool saved;
    protected Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
		saved = false;
    }    

    public override void UseAction() {
		//Prevent the swimmer from beign saved twice
		if (saved == true) return;

		animator.Play("SavedSwimmer");
		saved = true;
		GameObject.FindObjectOfType<LevelManager>().saveCrates(1);
    }
}
