using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : Character
{
    public int maxHealth = 100;
    // [SyncVar]
    public int currentHealth;
    public HealthBar healthBar;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!isLocalPlayer)
            return;
        base.Update();
    }

    // [Command]
    [ClientRpc]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
