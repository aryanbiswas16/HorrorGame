using System.Collections;
using UnityEngine;
// Adjust the namespace according to your Unity version and installed packages


public class ActivateChaseScript : MonoBehaviour
{
    public GameObject monster; // Assign this in the Inspector with your monster GameObject
    public GameObject globalLightGameObject; // Assign the global light GameObject in the inspector

    private UnityEngine.Rendering.Universal.Light2D globalLight; // For direct manipulation of the Light 2D component

    private void Start()
    {
        // Attempt to get the Light2D component from the assigned globalLightGameObject
        globalLight = globalLightGameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        // Set the initial intensity of the light to 0
        if (globalLight != null)
        {
            globalLight.intensity = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            monster.SetActive(true); // Activate the monster object
            StartCoroutine(FlashLight()); // Start the light flashing coroutine
        }
    }

    private bool stopFlashing = false;

    public void StopFlashingLight()
    {
        stopFlashing = true;
        // Optionally, ensure the light has a specific intensity when stopping
        if (globalLight != null)
        {
            globalLight.intensity = 0f; // Adjust to desired intensity
        }
    }

    IEnumerator FlashLight()
    {
        if (globalLight == null) yield break;

        globalLight.color = Color.red;

        while (!stopFlashing)
        {   
            globalLight.color = Color.red;
            globalLight.intensity = 0f; // Light off
            yield return new WaitForSeconds(0.3f);
            globalLight.intensity = 1f; // Light on (adjust intensity as needed)
            yield return new WaitForSeconds(0.3f);
        }

        // Ensure the light has a specific intensity when stopping the flashing
        globalLight.intensity = 0f; // Adjust to default or desired intensity
        globalLight.color = Color.white;
    }
}
