using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToEndScene : MonoBehaviour
{
    // You can change the scene name through the Inspector if you like

    public string sceneName = "EndScene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure your player GameObject has the tag "Player"
        {   
                        SceneManager.LoadScene(sceneName);
        }
    }
}