using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CopterEntryScript : MonoBehaviour {    

    public int index;						//The index of the copter in the gamemanagers copter list
	public Image copterImage;				//The image of the copter to show in the selection list. Get the sprite from gamemanager

	private CopterSelection copterSelect;	//Reference to the copterselect script

	private Image backgroundImage;			//The background sprite. Turn off if the copter is not selected
    private GameManager gameManager;		//Reference to the game manager
	private CopterInfo copter;				//The copter the button will show

	public void SetInfo(int index, CopterSelection select, CopterInfo copter) {

		backgroundImage = GetComponent<Image> ();

		this.index = index;
		this.copter = copter;
		copterSelect = select;
		gameManager = GameObject.FindObjectOfType<GameManager> ();

		copterImage.sprite = copter.copterSprite;
	}
}
