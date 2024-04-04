using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BalanceGameController : MonoBehaviour
{
    public Slider balanceSlider; // Assign this in the inspector
    public float balanceSpeed = 1f; // The speed at which the balance changes with input
    public float edgeForce = 0.1f; // The force that moves the slider towards the edges over time
    private float targetValue; // The target value the slider moves towards

    public GameObject healthManagerObject; // Assign in inspector
    private HealthManager healthManager; // The HealthManager component

    private float gameDuration = 5f; // The duration of the game in seconds
    private bool gameActive = false; // Tracks if the game is currently active

    void OnEnable()
    {
        ResetGame(); // Reset and start the game whenever the GameObject is enabled
    }

    void Update()
    {
        if (!gameActive) return; // If the game is not active, don't process input or movement

        float input = 0f; // The input value to change the balance

        // Get input from the left and right arrows
        if (Input.GetKey(KeyCode.Q))
        {
            input = -balanceSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            input = balanceSpeed * Time.deltaTime;
        }

        // Apply the edge force if no input is given, to move the ball towards the edges
        if (input == 0f)
        {
            if (targetValue < 0.5f)
            {
                input = -edgeForce * Time.deltaTime;
            }
            else
            {
                input = edgeForce * Time.deltaTime;
            }
        }

        // Apply the input (or edge force) to the target value and clamp it
        targetValue = Mathf.Clamp(targetValue + input, 0f, 1f);

        // Update the slider's value
        balanceSlider.value = targetValue;
    }

    IEnumerator EndGameAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration); // Wait for the game duration to pass
        gameActive = false; // Set the game as inactive

        // Check the slider value to determine if a heart should be removed
        if (balanceSlider.value < 0.40f || balanceSlider.value > 0.60f)
        {
            healthManager?.RemoveHeart(); // Remove a heart if the health manager exists and condition is met
        }

        gameObject.SetActive(false); // Disable the GameObject
    }

    void ResetGame()
    {
        // Start the target value off to the right
        targetValue = 0.75f; // Adjust as needed to start further to the right
        balanceSlider.value = targetValue;

        // Get the HealthManager component from the assigned GameObject
        healthManager = healthManagerObject.GetComponent<HealthManager>();

        gameActive = true; // Set the game to active

        // Start the coroutine to end the game after the set duration
        StartCoroutine(EndGameAfterTime(gameDuration));
    }
}