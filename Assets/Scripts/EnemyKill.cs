using UnityEngine;
using UnityEngine.UI; // Include this if you're using UI components
using UnityEngine.SceneManagement; // Include this for scene management
using System.Collections;


public class GameOverTrigger : MonoBehaviour
{
    public GameObject gameOverSign; 
    private bool isPlayerInContact = false;
    private float contactTime = 0.75f; 
    
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
            gameOverSign.SetActive(true); 
        }
    }
}
