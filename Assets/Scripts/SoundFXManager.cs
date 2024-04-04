using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    // Singleton instance
    private static SoundFXManager instance;

    // AudioSource component for playing sound effects
    private AudioSource audioSource;

    // Initialize the singleton instance
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Play a sound effect
    public void Play(AudioClip soundEffect, float volume)
    {
        audioSource.PlayOneShot(soundEffect, volume);
    }

    // Static method to access the singleton instance
    public static SoundFXManager GetInstance()
    {
        return instance;
    }
}
