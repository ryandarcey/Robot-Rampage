using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using StarterAssets;

public class PlayerStats : MonoBehaviour
{
    // Starting and max health for player
    public float health = 95f;
    public float healthMax = 100f;
    // Starting and max ammo for player
    public float ammo = 20f;
    public float ammoMax = 50f;
    
    // UI elements for health and ammo
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;

    // Player GameObjects
    GameObject player;

    // Timer for Powerup
    private float timeSinceItem;
    private float timeForItem;

    // Round manager
    public GameObject roundManagerGO;
    private RoundManager roundManager;

    // Set up UI and RoundManager 
    public void Start()
    {
        player = GameObject.Find("PlayerArmature");

        roundManager = roundManagerGO.GetComponent<RoundManager>();
        ammoText.color = Color.white;
        ammoText.text = "No PowerUp";
        //updateUI();
    }

    private void Update()
    {
        // Update time since any points were added. Change color to default white after enough time passed
        if (timeSinceItem >= 0)
        {
            timeSinceItem += Time.deltaTime;
            if (timeSinceItem > timeForItem)
            {
                resetValues();

                ammoText.color = Color.white;
                ammoText.text = "No PowerUp";
                timeSinceItem = -1f;
            }
        }
    }

    // Update the player's health after taking damage
    public void isHit(float damage)
    {
        // Lower health and points
        //health -= damage;
        FindObjectOfType<ScoreManager>().addPoints(-50);

        // Play sound
        FindObjectOfType<AudioManager>().PlaySound("player damage");

        FindObjectOfType<LogManager>().writeLog("Player Hit");
        //Debug.Log("Current health: " + health);

        //updateUI();

        // End round if the player has no health left
        if (health <= 0)
        {
			roundManager.RoundLose();
		}
    }

    // Reduce the amount of ammo after a shot
    public float loseAmmo()
    {
/*        if (ammo > 0)
        {
            ammo--;
            //Debug.Log("Ammo: " + ammo);

            updateUI();
        }*/

        return ammo;
    }

    // Update specific player stats based on the picked up item
    public void collectItem(int itemType, float value)
    {
        // If the item was a health pack, increase the health up to the max value
        switch(itemType)
        {
            // Health
            case 0:
                health += value;
                if (health > healthMax)
                {
                    health = healthMax;
                }

                FindObjectOfType<LogManager>().writeLog("Collected Health");
                break;
            // Ammo
            case 1:
                ammo += value;
                if (ammo > ammoMax)
                {
                    ammo = ammoMax;
                }

                FindObjectOfType<LogManager>().writeLog("Collected Ammo");
                break;
            // Speed
            case 2:
                resetValues();

                timeSinceItem = 0;
                timeForItem = 10f;
                ammoText.text = "Speed Up";
                ammoText.color = Color.cyan;

                // Change speed in ThirdPersonController script
                player.GetComponent<ThirdPersonController>().MoveSpeed = 18f;
                player.GetComponent<ThirdPersonController>().SprintSpeed = 24f;

                FindObjectOfType<LogManager>().writeLog("Collected Speed Up");
                break;
            // Power
            case 3:
                resetValues();

                timeSinceItem = 0;
                timeForItem = 7f;
                ammoText.text = "Damage Up";
                ammoText.color = Color.yellow;

                // Change damage in PlayerAction script
                player.GetComponent<PlayerAttack>().damage = 10;

                FindObjectOfType<LogManager>().writeLog("Collected Damage Up");
                break;
        }

        // Play sound
        FindObjectOfType<AudioManager>().PlaySound("player pickup");

        //updateUI();

    }

    // Update UI elements related to player
    void updateUI()
    {
        healthText.text = "Health: " + health + " / " + healthMax.ToString();
        ammoText.text = "Ammo: " + ammo + " / " + ammoMax.ToString();
    }

    private void resetValues()
    {
        // Speed
        player.GetComponent<ThirdPersonController>().MoveSpeed = 12f;
        player.GetComponent<ThirdPersonController>().SprintSpeed = 18f;

        // Damage
        player.GetComponent<PlayerAttack>().damage = 5;
    }

}
