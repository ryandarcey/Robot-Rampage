using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBehavior : MonoBehaviour
{
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
        // 3 represents the ground layer
        else if (collision.gameObject.layer == 3 || collision.gameObject.name.Contains("Wall"))
        {
            //Debug.Log("Hit ground");
            Destroy(gameObject);
        }
    }

    void hitPlayer(Collision collision)
    {

        PlayerHealth health = collision.gameObject.transform.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.isHit(damage);
        }

        Debug.Log("Hit player");
    }
}
