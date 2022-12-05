using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{

    public float health = 10f;

    // Animator for enemy
    public Animator animator;

    // Gameobject used for attack projectile
    public GameObject enemyProjectile;

    // Gameobjects used for health and ammo packs
    public GameObject healthPack;
    public GameObject ammoPack;

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
        // Rotates object to properly shoot towards player. Angle changes randomly for enemy accuracy
        shot.MoveRotation(Quaternion.Euler(0, Random.Range(-100, -85), 0));
        // Push object forward towards player
        shot.AddRelativeForce(transform.forward * 40f, ForceMode.Impulse);
        shot.AddForce(transform.up * 1f, ForceMode.Impulse);
    }

    public void isHit(float damage)
    {
        health -= damage;
        if (health <= 0f && !isDestroyed)
        {
            isDestroyed = true;
            dropPack();
            animator.SetTrigger("destroyed");
            //GetComponentInChildren<BoxCollider>().enabled = false;
        }
    }
 
    // Randomly determines whether or not to drop an item pack, then chooses which one
    void dropPack()
    {
        int randomNum = Random.Range(0, 2);

        Rigidbody pack;

/*        if (randomNum == 1) {
            randomNum = Random.Range(0, 1);*/
            if(randomNum == 1)
            {
                pack = Instantiate(healthPack, projectilePosition.position, Quaternion.identity).GetComponent<Rigidbody>();
            }
            else
            {
                pack = Instantiate(ammoPack, projectilePosition.position, Quaternion.identity).GetComponent<Rigidbody>();
            }
        }
    //}
}
