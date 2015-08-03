using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionButton : MonoBehaviour {

    public GameObject actionItem;       //The prefab to instantiate
    public LevelManager manager;        
    public Text actionText;             //Show how many actions are left
    public int maxActions = 10;         //How many actions are available at start
    private int actionsLeft;            //How many actions left

    private Transform copterAnchor;     //The position to instantiate the item
    private Rigidbody2D copterRb;       //The rigidbody of the copter

	void Start () {
		manager = GameObject.FindObjectOfType<LevelManager> ();
		Debug.Log (manager.levelAction);
        if (!(manager.levelAction > 0))
            transform.parent.gameObject.SetActive(false);
        copterAnchor = GameObject.Find("Copter").transform;
        copterRb = GameObject.Find("Copter").GetComponent<Rigidbody2D>();
        actionsLeft = maxActions;

        actionText.text = actionsLeft.ToString() + "/" + maxActions.ToString();
	}
	
    public void UseAction() {
        if (actionsLeft >= 1) {
            actionsLeft--;
            GameObject tempActionObject = null;
            tempActionObject = Instantiate(actionItem, copterAnchor.transform.position, Quaternion.identity) as GameObject;
            Rigidbody2D rb = tempActionObject.GetComponent<Rigidbody2D>();
            rb.AddForce(copterRb.velocity);
            rb.AddTorque(2f * (Random.value - 0.5f));
            actionText.text = actionsLeft.ToString() + "/" + maxActions.ToString();
        }
    }
}
