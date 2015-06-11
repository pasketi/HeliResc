using UnityEngine;
using System.Collections;

/// <summary>
/// Contains all the information about copters state such as health and interactions with other objects.
/// </summary>
[System.Serializable]
public class CopterHealth : Upgradable {
    
    public GameObject explosionPrefab;
    public GameObject brokenCopterPrefab;    

    public float maxHealth;

    //Getters
    public float CurrentHealth { get { return currentHealth; } }
    
    public float currentHealth;

    public override void Init(Copter copter) {
        base.Init(copter);
        
        currentHealth = maxHealth;        
               
    }    

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        Debug.Log("Current health: " + currentHealth);
        if (currentHealth <= 0)
            Explode();
    }
    public void FixCopter() {
        currentHealth = maxHealth;
    }    

    protected virtual void Explode() {        
        GameObject newCopter = playerCopter.CreateGameObject(brokenCopterPrefab, playerRb.transform.position, playerRb.transform.rotation);
        playerCopter.CreateGameObject(explosionPrefab, playerRb.transform.position, Quaternion.identity);
        Rigidbody2D[] parts = newCopter.GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D part in parts) {
            part.velocity += playerRb.velocity;
            part.angularVelocity += playerRb.angularVelocity;
        }
        newCopter.GetComponent<ExplodeParts>().enabled = true;
        currentHealth = 0f;        

        playerCopter.Disable();
        EventManager.TriggerEvent("CopterExplode");
    }
    protected override void GiveName() {
        name = "CopterHealth";
    }
}
