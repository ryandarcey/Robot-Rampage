using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBehavior : MonoBehaviour
{
    // Damage dealt when colliding with player
    public float damage;

    // Layer for checking ground collisions
    //public LayerMask groundLayer;

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PlayerArmature")
        {
            Destroy(gameObject);
            hitPlayer(collision);
        }
        // 3 represents the ground layer
        else if (collision.gameObject.layer == 3 || collision.gameObject.name.Contains("Wall"))
        {
            //Debug.Log("Hit ground");
            Destroy(gameObject);
        }
    }

    void hitPlayer(Collision collision)
    {

        PlayerStats health = collision.gameObject.transform.GetComponent<PlayerStats>();
        if (health != null)
        {
            Debug.Log("Hit player");
            health.isHit(damage);
        }
    }
}
