using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal; // Update this based on your Unity version

public class SimulateLightning : MonoBehaviour
{
    public SoundFXManager soundFXManager;
    
    
    private Light2D globalLight; // Reference to the Light2D component on this GameObject
    public Color lightningColor = Color.red; // The color of the lightning
    public float minFlashDelay = 5f; // Minimum delay between flashes
    public float maxFlashDelay = 10f; // Maximum delay between flashes
    public float lightningDuration = 0.2f; // Duration of each lightning flash

    private Color originalColor; // To store the original color of the light
    private float originalIntensity = 0f; // The original intensity is explicitly set to 0

    private void Awake()
    {
        // Get the Light2D component from this GameObject
        globalLight = GetComponent<Light2D>();

        // Initialize the light's state
        InitializeLightState();
    }

    private void InitializeLightState()
    {
        // Store the original color of the light
        originalColor = globalLight.color;

        // Ensure the light starts with an intensity of 0
        globalLight.intensity = originalIntensity;
    }

    private void Start()
    {
        soundFXManager = SoundFXManager.GetInstance();
        // Start the lightning effect coroutine
        StartCoroutine(LightningEffect());
    }

    IEnumerator LightningEffect()
    {
        while (true)
        {
            // Wait for a random time before the next lightning flash
            yield return new WaitForSeconds(Random.Range(minFlashDelay, maxFlashDelay));

            // Change the light color and increase intensity for the lightning effect
            globalLight.color = lightningColor;
            globalLight.intensity = 1f; // Set to desired max intensity during flash

            AudioClip attackSound = Resources.Load<AudioClip>("Sounds/Clips/Lighting-strike");
            soundFXManager.Play(attackSound, 0.1f);
            // Wait for the duration of the lightning effect
            yield return new WaitForSeconds(lightningDuration);

            // Restore the light to its original state (color and 0 intensity)
            globalLight.color = originalColor;
            globalLight.intensity = originalIntensity;
        }
    }
}
