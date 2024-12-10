using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHand : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider col)
    {
        // Log the name of the object that collided with ZombieHand
        Debug.Log("Collided with: " + col.gameObject.name);

        // Check if the object has the "Player" tag
        if (col.CompareTag("Player"))
        {
            // Attempt to get the Player component from the collided object
            Player player = col.GetComponent<Player>();

            // Log if player component is found or not
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("Player component not found on " + col.gameObject.name);
            }
        }
        else
        {
            Debug.Log("Collided object is not the Player.");
        }
    }
}