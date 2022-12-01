using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // Starting health for player
    public float health = 15f;

    // Update is called once per frame
    public void isHit(float damage)
    {

        health -= damage;

        Debug.Log("Current health: " + health);

        if (health <= 0)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
