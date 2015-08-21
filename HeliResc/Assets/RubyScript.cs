using UnityEngine;
using System.Collections;

public class RubyScript : MonoBehaviour {

    public bool found;
    public bool IsSapphire { get { return isSapphire; } }
    public Sprite sapphireSprite;

    private bool isSapphire;
    private Animator animator;
    //private string rubyName;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();

        isSapphire = LevelHandler.CurrentLevel.rubyFound;
        if (isSapphire == true) {
            animator.Play("GlowingSapphire");
        }

//        rubyName = Application.loadedLevelName + "Ruby";
//        if (PlayerPrefs.HasKey(rubyName) == false) {
//            PlayerPrefsExt.SetBool(rubyName, false);
//        }
//        else if (PlayerPrefsExt.GetBool(rubyName) == true) {
//#if !UNITY_EDITOR
//            gameObject.SetActive(false);
//#endif
//        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Copter") || other.tag.Equals("Hook")) {
            found = true;

            if (isSapphire == false)
            {
                //PlayerPrefsExt.SetBool(rubyName, true);
                animator.Play("Collected");
            }
            else
            {
                animator.Play("CollectSapphire");
            }
        }
    }

}
