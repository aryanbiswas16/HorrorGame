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
            DontDestroyOnLoad(gameObject); // Keep the object alive between scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Play a sound effect
    public void Play(AudioClip soundEffect)
    {
        audioSource.PlayOneShot(soundEffect);
    }

    // Static method to access the singleton instance
    public static SoundFXManager GetInstance()
    {
        return instance;
    }
}