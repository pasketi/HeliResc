using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FingerAnimation : MonoBehaviour {

    TutorialScript tutorial;

    public RectTransform finger;
    public RectTransform copter;

    private Vector2 fingerOriginalPos;
    private Vector2 copterOriginalPos;
    private Vector2 fingerTargetPos;
    private Vector2 copterTargetPos;
    private Vector2 fingerSize;
    private Vector2 copterSize;

    private Vector2 distance = new Vector2(0, 0.5f);
    private float speed;

    private bool moveCopter;

    private bool showVertical;

    public bool finished;

    // Use this for initialization
    void Start () {
        fingerOriginalPos = finger.anchorMin;
        copterOriginalPos = copter.anchorMin;
        fingerSize = finger.anchorMax - finger.anchorMin;
        copterSize = copter.anchorMax - copter.anchorMin;

        fingerTargetPos = fingerOriginalPos + distance;
        copterTargetPos = copterOriginalPos + distance;

        showVertical = true;
        moveCopter = true;
        finished = false;

        tutorial = GameObject.Find("Tutorial").GetComponent<TutorialScript>();

        speed = 0.005f;

        Invoke("MoveFingerVertical", 1);
        Invoke("MoveCopterVertical", 1.5f);
    }

    void Update() {
        if (showVertical) GetVerticalInput();
        else GetHorizontalInput();

       
    }

    public void ClickedOK() {
        if (showVertical)
            SwitchToHorizontal();
        else
        {
            finished = true;
            gameObject.SetActive(false);
        }
    }

    private void SwitchToHorizontal() {
        CancelInvoke();
        Vector2 newPos = new Vector2(fingerOriginalPos.y, fingerOriginalPos.x);
        fingerOriginalPos = newPos;

        newPos.y = 0.4f;
        copterOriginalPos = newPos;

        distance = new Vector2(0.6f, 0);
        fingerTargetPos = fingerOriginalPos + distance;
        copterTargetPos = copterOriginalPos + distance;

        showVertical = false;
        moveCopter = true;

        finger.anchorMin = fingerOriginalPos;
        finger.anchorMax = finger.anchorMin + fingerSize;

        copter.anchorMin = copterOriginalPos;
        copter.anchorMax = copter.anchorMin + copterSize;

        Invoke("MoveFingerHorizontal", 1);
        Invoke("MoveCopterHorizontal", 1.5f);
    }

    private void MoveFingerHorizontal()
    {
        if (finger.anchorMin.x < fingerTargetPos.x)
        {
            finger.anchorMin += Vector2.right * speed;
            finger.anchorMax = finger.anchorMin + fingerSize;
        }
        else
        {
            Invoke("ResetFinger", 1);
            Invoke("MoveFingerHorizontal", 1);
            return;
        }

        Invoke("MoveFingerHorizontal", speed * 4f);
    }

    private void MoveCopterHorizontal()
    {
        if (copter.anchorMin.x < copterTargetPos.x && moveCopter)
        {
            copter.anchorMin += Vector2.right * speed;
            copter.anchorMax = copter.anchorMin + copterSize;

            copter.transform.Rotate(new Vector3(0, 0, -speed * 60));

        }
        else
        {
            moveCopter = false;            
        }

        Invoke("MoveCopterHorizontal", speed * 4f);

    }

    private void MoveFingerVertical() {
        if (finger.anchorMin.y < fingerTargetPos.y)
        {
            finger.anchorMin += Vector2.up * speed;
            finger.anchorMax = finger.anchorMin + fingerSize;
        }
        else {
            Invoke("ResetFinger", 1);
            Invoke("MoveFingerVertical", 1);
            return;
        }

        Invoke("MoveFingerVertical", speed * 4f);        
    }

    private void MoveCopterVertical()
    {
        if (copter.anchorMin.y < copterTargetPos.y && moveCopter)
        {
            copter.anchorMin += Vector2.up * speed;
            copter.anchorMax = copter.anchorMin + copterSize;            
        }
        else
        {            
            moveCopter = false;
        }
        Invoke("MoveCopterVertical", speed * 4f);

    }

    private void ResetCopter() {
        moveCopter = true;
    }

    private void ResetFinger() {
        finger.anchorMin = fingerOriginalPos;
        finger.anchorMax = finger.anchorMin + fingerSize;

        copter.anchorMin = copterOriginalPos;
        copter.anchorMax = copter.anchorMin + copterSize;

        copter.transform.rotation = Quaternion.Euler(Vector3.zero);

        Invoke("ResetCopter", 0.5f);
    }

    private void GetVerticalInput() {
        
    }

    private void GetHorizontalInput()
    {

    }
}
