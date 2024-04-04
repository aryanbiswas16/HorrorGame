using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes
using UnityEngine.UI; // Required for UI elements like Text

public class ContinueButtonScript : MonoBehaviour
{
    public GameObject[] textBoxes; // Assign your text boxes in the Inspector.
    public AudioClip firstClickSound; // Assign your sound clip in the Inspector.
    private AudioSource audioSource; // Reference to the AudioSource component.
    private int clickCount = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component.
        // Ensure all text boxes are initially hidden.
        foreach (var textBox in textBoxes)
        {
            textBox.SetActive(false);
        }
    }

    public void OnContinueClicked()
    {
        if (clickCount < textBoxes.Length)
        {
            // Display one text box per click.
            textBoxes[clickCount].SetActive(true);

            // Play the sound only on the first click.
            if (clickCount == 0)
            {
                if (audioSource != null && firstClickSound != null)
                {
                    audioSource.PlayOneShot(firstClickSound); // Play the sound.
                }
                else
                {
                    Debug.LogWarning("AudioSource or AudioClip is missing.");
                }
            }

            clickCount++;
        }
        else
        {
            // After all text boxes have been displayed, load the next scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
