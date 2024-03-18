using UnityEngine;
using UnityEngine.UI; // Include this if you're using UI components
using UnityEngine.SceneManagement; // Include this for scene management
using System.Collections;


public class GameOverTrigger : MonoBehaviour
{
    public GameObject gameOverSign; // Assign this in the inspector with your Game Over UI element
    private bool isPlayerInContact = false;
    private float contactTime = 1.0f; // Duration the player must stay in contact to trigger game over

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInContact = true;
            StartCoroutine(CheckContactDuration());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInContact = false;
        }
    }

    private IEnumerator CheckContactDuration()
    {
        yield return new WaitForSeconds(contactTime);

        if (isPlayerInContact)
        {
            gameOverSign.SetActive(true); // Show the Game Over sign
            // Here you can also handle anything else you want to do when the game is over, such as:
            // SceneManager.LoadScene("YourGameOverSceneName"); // Load a Game Over scene
            // OR
            // Time.timeScale = 0; // Freeze the game
        }
    }
}
