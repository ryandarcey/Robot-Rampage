using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{

    // Stats for enemy. Changes depending on the enemy type
    public float health = 10f;
    public float damage;
    public float chaseSpeed;
    public float chaseRange;
    public float attackRange;
    public bool canChase;
    public bool canAttack;

    // Particle system for enemy
    public ParticleSystem sparks;

    // Animator for enemy
    public Animator animator;

    // Gameobject used for attack projectile
    public GameObject enemyProjectile;

    // Gameobjects used for health and ammo packs
    public GameObject speedPack;
    public GameObject damagePack;

    // Changes the range within which enemies shoot
    public float horizontalRange = 15; 

    // Position where projectile is instantiated
    public Transform projectilePosition;

    // Sets when the enemy has been destroyed
    bool isDestroyed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (animator == null)
        {
            animator = transform.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    public void Shoot()
    {
        // Shoots out a rigidbody as a projectile
        Rigidbody shot = Instantiate(enemyProjectile, projectilePosition.position, Quaternion.identity).GetComponent<Rigidbody>();
        shot.GetComponent<EnemyProjectileBehavior>().damage = damage;

        // Rotates object to properly shoot towards player. Angle changes randomly for enemy accuracy
        shot.MoveRotation(Quaternion.Euler(0, Random.Range(-95 - horizontalRange, -95 + horizontalRange), 0));

        // Push object forward towards player
        shot.AddRelativeForce(transform.forward * 40f, ForceMode.Impulse);
        shot.AddForce(transform.up * 1f, ForceMode.Impulse);

        // Play sound
        FindObjectOfType<AudioManager>().PlaySound("enemy attack");
    }

    public void isHit(float damage)
    {
        health -= damage;
        sparks.Play();
        if (health <= 0f && !isDestroyed)
        {
            // Add points
            FindObjectOfType<ScoreManager>().addPoints(100);

            isDestroyed = true;

            // Play random death sound
            int num = Random.Range(1, 4);
            FindObjectOfType<AudioManager>().PlaySound("enemy death " + num.ToString());

            dropPack();
            animator.SetTrigger("destroyed");
            //GetComponentInChildren<BoxCollider>().enabled = false;
        }
        else
        {
            // Play sound
            FindObjectOfType<AudioManager>().PlaySound("enemy damage");
        }
    }
 
    // Randomly determines whether or not to drop an item pack, then chooses which one
    void dropPack()
    {
        // 66% chance for an item to drop
        int randomNum = Random.Range(0, 4);

        Rigidbody pack;

        switch(randomNum)
        {
            case 0:
                pack = Instantiate(speedPack, projectilePosition.position, Quaternion.identity).GetComponent<Rigidbody>();
                break;
            case 1:
                pack = Instantiate(damagePack, projectilePosition.position, Quaternion.identity).GetComponent<Rigidbody>();
                break;
            default:
                break;
        }
    }
}
