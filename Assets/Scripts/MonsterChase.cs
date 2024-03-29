using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChase : MonoBehaviour
{
    public Transform teleportLine; // Assign the TeleportLine object in the inspector
    public GameObject gameOverSign; // Assign the GameOver sign object in the inspector
    public float speed = 2f; // Adjust speed as necessary

    private void Update()
    {
        // Move the monster towards the TeleportLine each frame
        transform.position = Vector2.MoveTowards(transform.position, teleportLine.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // If the monster collides with the player, activate the game over sign and potentially stop the game
            gameOverSign.SetActive(true);
            // Here you can add any additional game over logic, such as freezing the game or displaying a restart button
        }
        // Optionally, handle what happens if the monster reaches the TeleportLine without catching the player
    }
}
