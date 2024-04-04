using UnityEngine;
using UnityEngine.UI; // Include this if you're using UI components
using UnityEngine.SceneManagement; // Include this for scene management
using System.Collections;


public class GameOverTrigger : MonoBehaviour
{
    public GameObject gameOverSign; 
    private bool isPlayerInContact = false;
    private float contactTime = 0.75f;

    private SoundFXManager soundFXManager;
    private AudioSource audioSource;

    private void Start()
    {
        soundFXManager = SoundFXManager.GetInstance();
    }
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
            AudioClip deathSound = Resources.Load<AudioClip>("Sounds/Clips/Instant-Death");
            if (deathSound == null)
            {
                Debug.LogError("Sound file not found");
            }else
            {
                soundFXManager.Play(deathSound, 1f);
            }
            
            gameOverSign.SetActive(true); 
            Time.timeScale = 0;
        }
    }
}
