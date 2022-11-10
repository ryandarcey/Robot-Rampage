using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{

    // Damage and range for the current gun
    public float damage = 5f;
    public float range = 30f;

    // Layer for checking enemy collisions
    public LayerMask enemyLayer;

    // Crosshair for player
    public Image crosshair;

    // Player camera
    public Camera camera;

    // Check if player can shoot an enemy
    bool canShoot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Information about the item hit by the Raycast
        RaycastHit hitInformation;

        // Checks if the raycast from player collides with an enemy, then allows them to shoot if so and changes the crosshair color
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInformation, range, enemyLayer)) 
        {
            canShoot = true;
            crosshair.color = Color.red;
        }
        else
        {
            canShoot = false;
            crosshair.color = Color.black;
        }

        // Shoots the gun
        if (Input.GetButtonDown("Fire1"))
        {
            ShootGun(hitInformation);
        }
    }

    void ShootGun(RaycastHit hitInformation)
    {
        // Only deal damage if the player is shooting at an enemy
        if (canShoot)
        {
            EnemyBehavior enemy = hitInformation.transform.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                enemy.isHit(damage);
            }
        }
    }
}
