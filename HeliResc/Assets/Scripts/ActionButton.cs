using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionButton : MonoBehaviour {

    public GameObject actionItem;       //The prefab to instantiate
    public LevelManager manager;        
    public Text actionText;             //Show how many actions are left
    public int maxActions = 10;         //How many actions are available at start
    private int actionsLeft;            //How many actions left

    private System.Action actionMethod; //The method to run depending on the level type
    private Transform copterAnchor;     //The position to instantiate the item
    private Rigidbody2D copterRb;       //The rigidbody of the copter
    private BucketScript bucket;

	void Start () {
		
        copterAnchor = GameObject.Find("Copter").transform;
        copterRb = GameObject.Find("Copter").GetComponent<Rigidbody2D>();
        actionMethod = () => { };

        manager = GameObject.FindObjectOfType<LevelManager> ();
        int type = manager.levelAction;
        if (type == 0){
            transform.parent.gameObject.SetActive(false);
        }
        else if (type == 1)
        {
            actionsLeft = maxActions;
            actionText.text = actionsLeft.ToString() + "/" + maxActions.ToString();
            actionMethod = ThrowLifeRing;
        }
        else if (type == 2) {
            actionText.enabled = false;
            actionMethod = ThrowWater;            
        }
	}
	
    public void UseAction() {
        actionMethod();
    }
    private void ThrowLifeRing() {
        if (actionsLeft >= 1)
        {
            actionsLeft--;
            GameObject tempActionObject = null;
            tempActionObject = Instantiate(actionItem, copterAnchor.transform.position, Quaternion.identity) as GameObject;
            Rigidbody2D rb = tempActionObject.GetComponent<Rigidbody2D>();
            rb.AddForce(copterRb.velocity);
            rb.AddTorque(2f * (Random.value - 0.5f));
            actionText.text = actionsLeft.ToString() + "/" + maxActions.ToString();
        }
    }

    private void ThrowWater() {
        if(bucket == null)
            bucket = GameObject.FindObjectOfType<BucketScript>();
        if (bucket == null)
            Debug.LogError("Bucket not found");
        else
            bucket.Throw();
    }
}