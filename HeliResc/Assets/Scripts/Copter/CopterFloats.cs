using UnityEngine;
using System.Collections;

[System.Serializable]
public class CopterFloats : CopterWaterManager {    

    protected override void SplashUpdate() {
        if ((playerCopter.transform.position.y) < playerCopter.levelManager.getWaterLevel() + 0.3f) {
            playerRb.AddForce(Vector2.up * (playerRb.mass * 20f));
        }
    }

    protected override void GiveName() {
        name = "CopterFloats";
    }
}
