using UnityEngine;
using System.Collections;

public class RubyScript : MonoBehaviour {

    private Animator animator;
    private string rubyName;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        rubyName = Application.loadedLevelName + "Ruby";
        if (PlayerPrefs.HasKey(rubyName) == false) {
            PlayerPrefsExt.SetBool(rubyName, false);
        }
        else if (PlayerPrefsExt.GetBool(rubyName) == true) {
#if !UNITY_EDITOR
            gameObject.SetActive(false);
#endif
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Copter") || other.tag.Equals("Hook")) {
            PlayerPrefsExt.SetBool(rubyName, true);
            animator.Play("Collected");
        }
    }

}
