using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    // Starting and max health for player
    public float health = 95f;
    public float healthMax = 100f;
    // Starting and max ammo for player
    public float ammo = 20f;
    public float ammoMax = 50f;
    // Starting time for player
    public float time = 180f;
    double timeRounded;

    // UI elements for health and ammo
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI timeText;

    public void Start()
    {
        updateUI();
    }

    private void Update()
    {
        if(time <= 0)
        {
            endGame();
        }
        time -= Time.deltaTime;

        timeRounded = System.Math.Round(time, 2);
        timeText.text = "Time: " + timeRounded.ToString();
    }

    // Update is called once per frame
    public void isHit(float damage)
    {

        health -= damage;

        Debug.Log("Current health: " + health);

        updateUI();

        if (health <= 0)
        {
            endGame();
        }
    }

    public float loseAmmo()
    {
        if (ammo > 0)
        {
            ammo--;
            Debug.Log("Ammo: " + ammo);

            updateUI();

            return ammo;
        }
        else
        {
            return ammo;
        }
    }

    public void collectItem(bool isHealth, float value)
    {
        // If the item was a health pack, increase the health up to the max value
        if (isHealth)
        {
            health += value;
            if (health > healthMax)
            {
                health = healthMax;
            }
            Debug.Log("Collected Health, Health: " + health);
        }
        // If the item was an ammo pack, increase the health up to the max value
        else
        {
            ammo += value;
            if(ammo > ammoMax)
            {
                ammo = ammoMax;
            }
            Debug.Log("Collected Ammo, Ammo: " + ammo);
        }

        updateUI();

    }

    void updateUI()
    {
        healthText.text = "Health: " + health + " / " + healthMax.ToString();
        ammoText.text = "Ammo: " + ammo + " / " + ammoMax.ToString();
    }

    void endGame()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}