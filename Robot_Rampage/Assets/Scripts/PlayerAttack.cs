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
    public CanvasRenderer crosshair;

    // Player camera
    public Camera camera;

    // Sound effect for shooting
    public AudioSource shot;

    // Check if player can shoot an enemy
    bool canShoot;

    // Used to manage time between shots
    float coolDownTime = .5f;
    float nextShotTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

        if (crosshair == null)
        {
            crosshair = FindObjectsOfType<CanvasRenderer>()[0];
        }
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
            crosshair.SetColor(Color.red);
        }
        else
        {
            canShoot = false;
            crosshair.SetColor(Color.white);
        }

        // Shoots the gun if the cooldown time is over
        if (nextShotTime < Time.time)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                // Sets the next time a shot can be made and checks if a shot connected
                nextShotTime = coolDownTime + Time.time;
                ShootGun(hitInformation);
            }
        }
    }

    void ShootGun(RaycastHit hitInformation)
    {
        // Play sound
        //shot.Play();

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
