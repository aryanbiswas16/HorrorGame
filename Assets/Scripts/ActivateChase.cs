using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChaseScript : MonoBehaviour
{
    public GameObject monster; // Assign this in the Inspector with your monster GameObject

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Check if the collision object has the tag "Player"
            monster.SetActive(true); // Activate the monster object
        }
    }
}
