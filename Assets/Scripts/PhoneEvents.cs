using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickManager : MonoBehaviour
{
    public AudioSource audioSource; // Assign this in the inspector
    public GameObject Call; // Assign your Call GameObject here
    public HealthManager healthManager; // Assign your HealthManager component, if available
    public float audioPlayDuration = 5f; // Set this to the number of seconds you want the audio to play

    void Start()
    {
        // Get the Button component and add a listener to its click event
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnButtonClick);
        }
    }

    public void OnButtonClick()
    {
        StartCoroutine(PlaySoundForDurationThenTurnOff());
    }

    private IEnumerator PlaySoundForDurationThenTurnOff()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // Start playing the sound
            yield return new WaitForSeconds(audioPlayDuration); // Wait for the specified duration
            audioSource.Stop(); // Stop playing the sound
        }

        // Disable the Call GameObject if it's assigned
                                
        if (Call!= null)
        {
            Call.SetActive(false);
        }

        // Call RemoveHeart only if HealthManager is assigned
        if (healthManager != null)
        {
            healthManager.RemoveHeart();
        }
    }
}