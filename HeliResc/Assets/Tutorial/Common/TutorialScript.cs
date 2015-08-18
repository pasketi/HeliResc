using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {

	public Button okButton;     //Button to close the first tutorial screen

	protected Animator animator;

	// Use this for initialization
	protected virtual void Start () {
		animator = GetComponent<Animator>();
		okButton.interactable = false;
		Invoke("ActivateButton", 1);
		GameObject.Find("Copter").GetComponent<Copter>().Kinematic(true);
	}

	protected virtual void ActivateButton()
	{
		okButton.interactable = true;
	}
	public virtual void ClickedOK()
	{
		GameObject.Find("Copter").GetComponent<Copter>().Kinematic(false);
		gameObject.SetActive(false);
		GameObject.FindObjectOfType<LevelManager> ().StartGame ();
	}
}
public class Tutorial {
	public static IEnumerator FadeOut(SpriteRenderer sprite, float time) {
		Color c = sprite.color;
		while (sprite.color.a > 0) {
			c.a -= (time * Time.deltaTime);
			sprite.color = c;
			yield return null;
		}
	}
}