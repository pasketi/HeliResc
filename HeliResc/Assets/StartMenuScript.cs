using UnityEngine;
using System.Collections;

public class StartMenuScript : MonoBehaviour {

    public UnityEngine.UI.Text buttonText;
    private bool ah;    

    void Start() {
        if (PlayerPrefs.HasKey(SaveStrings.autoHoover))
        {
            ah = PlayerPrefsExt.GetBool(SaveStrings.autoHoover);
        }
        else { 
            ah = true;
            PlayerPrefsExt.SetBool(SaveStrings.autoHoover, ah);
        }
        buttonText.text = ah ? "ON" : "OFF";
    }

    public void AutoHoover() {
        ah = !ah;
        PlayerPrefsExt.SetBool(SaveStrings.autoHoover, ah);
        buttonText.text = ah ? "ON" : "OFF";
    }
}
