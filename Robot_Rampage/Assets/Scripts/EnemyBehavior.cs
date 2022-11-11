using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float health = 10f;

    // Gameobject used for attack projectile
    public GameObject enemyProjectile;

    // Range for enemy to attack player
    public float attackRange = 25f;

    // Layer for checking player collisions
    public LayerMask playerLayer;

    // Player character
    public Transform player;

    // Used to manage time between shots
    float coolDownTime = .4f;
    float nextShotTime = 0f;

    // Check whether or not a player is in range to be attacked
    bool inAttackRange;

    void Update()
    {
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if(inAttackRange)
        {
            attack();
        }
    }

    void attack()
    {
        // Face player for attacking
        transform.LookAt(player);

        // Shoots if the cooldown time is over
        if (nextShotTime < Time.time)
        {
            // Sets the next time a shot can be made and checks if a shot connected
            nextShotTime = coolDownTime + Time.time;

            // Shoots out a rigidbody as a projectile
            enemyProjectile.SetActive(true);
            Rigidbody shot = Instantiate(enemyProjectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            enemyProjectile.SetActive(false);
            shot.AddForce(transform.forward * 30f, ForceMode.Impulse);
            shot.AddForce(transform.up * 5f, ForceMode.Impulse);
        }
    }

    public void isHit (float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
