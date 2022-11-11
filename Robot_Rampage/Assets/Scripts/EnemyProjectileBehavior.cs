using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBehavior : MonoBehaviour
{

    // Player model for collision
    public Transform player;

    // Damage dealt when colliding with player
    public float damage = 5f;

    // Layer for checking ground collisions
    //public LayerMask groundLayer;

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PlayerModel")
        {
            hitPlayer(collision);
        }
        else if (collision.gameObject.name == "Ground")
        {
            Debug.Log("Hit ground");
            Destroy(gameObject);
        }
    }

    void hitPlayer(Collision collision)
    {

        PlayerHealth health = player.transform.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.isHit(damage);
        }

        Debug.Log("Hit player");
    }
}
