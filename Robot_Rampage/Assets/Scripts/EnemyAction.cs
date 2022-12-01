using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{

    public float health = 10f;

    // Gameobject used for attack projectile
    public GameObject enemyProjectile;

    // Position where projectile is instantiated
    public Transform projectilePosition;

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
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
