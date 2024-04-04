using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Needed for loading scenes, if using scenes for game over

public class HealthManager : MonoBehaviour
{
    public int health = 3;
    public Image[] hearts;

    public GameObject gameOverSign;
    void Awake()
    {
        health = 3;
    }

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void RemoveHeart()
    {
        health = Mathf.Max(0, health - 1);

        // Check if health is 0 and trigger game over
        if (health == 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // Logic for what happens when the game is over
        // For example, loading a game over scene, displaying a game over screen, etc.
        gameOverSign.SetActive(true);

        // Example: Load a scene named "GameOver"
        // Make sure "GameOver" scene is added in your Build Settings scene list
        // SceneManager.LoadScene("GameOver");
    }
}
