using UnityEngine;
using System.Collections;

[System.Serializable]
public class CopterWaterManager : Upgradable {

    private GameObject splashPrefab;

    public override void Init(Copter copter) {
        base.Init(copter);

        splashPrefab = (playerCopter.levelManager == null) ? null : playerCopter.levelManager.levelSplash;
        UpdateDelegate = SplashUpdate;
    }

    protected virtual void SplashUpdate() {
        if (playerRb.transform.position.y < playerCopter.levelManager.getWaterLevel() + 0.3f) {
            Splash();
        }
    }

    public virtual void Splash() {
        UpdateDelegate = () => { };
        Vector3 splashPos = new Vector3(playerCopter.transform.position.x, playerCopter.levelManager.getWaterLevel() + 0.5f);
        playerCopter.CreateGameObject(splashPrefab, splashPos, Quaternion.identity);
        playerCopter.SetInputActive(false);
        EventManager.TriggerEvent(SaveStrings.eCopterSplash);
    }
    protected override void GiveName() {
        name = "WaterManager";
    }
}
