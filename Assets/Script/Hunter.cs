using System;
using System.Collections;
using UnityEngine;

public class Hunter : Character
{
    // * VARIABLES
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;

    // attack movement
    public const int stateAttackSlash = 6;
    public const int stateAttckKick = 7;

    [HideInInspector]
    public bool isAttacking = false;

    public string[] hitTags = { "Player" };

    // * FUNCTIONS
    // public Hunter() { }

    protected override void Start()
    {
        base.Start(); // Call the parent class's Start method
        // Add additional functionality here specific to Hunter class
    }

    protected override void Update()
    {
        if (!isLocalPlayer)
            return;
        base.Update();
        // hunter moves
        moveHunter();
        AnimateHunterCharacterMovs();
    }

    // MOVEMENTS
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // public void moveHunter()
    // {
    //     if (Input.GetKeyDown(KeyCode.Z))
    //     {
    //         print("attacking");
    //         isAttacking = true;
    //         ContactFilter2D filter = new ContactFilter2D();
    //         filter.useTriggers = true;
    //         filter.SetLayerMask(enemyLayers | (1 << gameObject.layer));
    //         Debug.Log("enemylayers" + (enemyLayers | (1 << gameObject.layer)));
    //         filter.useLayerMask = true;
    //         filter.useDepth = false;
    //         filter.useNormalAngle = false;
    //         Collider2D[] hitEnemies = new Collider2D[5];
    //         int numColliders = Physics2D.OverlapCircle(
    //             attackPoint.position,
    //             attackRange,
    //             filter,
    //             hitEnemies
    //         );
    //         foreach (Collider2D enemy in hitEnemies)
    //         {
    //             Debug.Log("We hit " + enemy.name + enemy.GetType());
    //             if (Array.IndexOf(hitTags, enemy.gameObject.tag) == -1)
    //             {
    //                 return;
    //             }
    //             // will only consider 1 collider, hence will be run once
    //             if (enemy is CapsuleCollider2D)
    //             {
    //                 enemy.GetComponent<PlayerController>().TakeDamage(attackDamage);
    //             }
    //         }
    //     }
    //     else
    //     {
    //         isAttacking = false;
    //     }
    // }
    public void moveHunter()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            print("attacking");
            isAttacking = true;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
                attackPoint.position,
                attackRange,
                enemyLayers
            );

            foreach (Collider2D enemy in hitEnemies)
            {
                // Debug.Log("We hit " + enemy.name + enemy.GetType());
                if (Array.IndexOf(hitTags, enemy.gameObject.tag) == -1)
                {
                    return;
                }
                if (enemy.gameObject.tag == "Player")
                {
                    // get from parent
                    enemy.GetComponentInParent<PlayerController>()?.TakeDamage(attackDamage);
                    return;
                }
                enemy.GetComponent<HealthController>()?.TakeDamage(attackDamage);
            }
        }
        else
        {
            isAttacking = false;
        }
    }

    void AnimateAttackSlash()
    {
        if (isAttacking)
        {
            currentAnimateState = stateAttackSlash;
        }
    }

    void AnimateHunterCharacterMovs()
    {
        AnimateAttackSlash();
    }
}

/*
to cahgne health size for hunter
if hunter
get objects by tag player or add health to each player
get health component
change canvas size of each player
make the health bar follow the player
*/