using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Bool that dictates the type of item. True if it is health, false if it is ammo
    [SerializeField] private int itemType;
    [SerializeField] private float itemValue;

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Object collided: " + collision.gameObject.name);
        if (collision.gameObject.name == "PlayerArmature")
        {
            PlayerStats stats = collision.gameObject.transform.GetComponent<PlayerStats>();
            if (stats != null)
            {
                Debug.Log("Item collected");
                
                // Depending on item type, apply the item value to the correct stat
                stats.collectItem(itemType, itemValue);

                // Add points
                FindObjectOfType<ScoreManager>().addPoints(10);
                
                Destroy(gameObject);

            }

        }
    }
}
