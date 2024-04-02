using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class StomachPainQTE : MonoBehaviour
{
    public Player playerScript; // Reference to the Player script
    public Text speedDisplayText; // Reference to the Text element for displaying speed
    public bool qteActive = true; // Control the activation of QTE

    private float originalSpeed;
    private float maxSpeed = 6f;
    private bool qteRunning = false;

    void Update()
    {
        if (qteActive && !qteRunning)
        {
            StartQTE();
        }

        if (qteRunning && Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseSpeed();
        }
    }

    void StartQTE()
    {
        originalSpeed = playerScript.moveSpeed; // Store the original speed
        playerScript.moveSpeed = 1f; // Slow down the player
        UpdateSpeedDisplay(); // Update speed display initially
        qteRunning = true;
        Invoke("EndQTE", 10f); // Schedule the end of QTE in 10 seconds
    }

    void IncreaseSpeed()
    {
        if (playerScript.moveSpeed < maxSpeed-0.1)
        {
            playerScript.moveSpeed += 0.2f; // Increase speed
            UpdateSpeedDisplay(); // Update the speed display
        }
    }

    void EndQTE()
    {
        playerScript.moveSpeed = originalSpeed; // Reset speed to original
        qteActive = false; // Optionally turn off QTE if it's a one-time event
        qteRunning = false;
        // Update the message to indicate the end of the stomach pain
        speedDisplayText.text = "You no longer feel the stomach pain.";
        // Invoke the method to hide the text after 3 seconds
        Invoke("HideSpeedDisplay", 4f);
    }

    void UpdateSpeedDisplay()
    {
        float speedPercentage = (playerScript.moveSpeed / 3f) * 100f; // Calculate speed as a percentage of the base speed
        speedDisplayText.text = $"SPAM \"SPACE\" TO ENDURE STOMACH PAIN! \nSpeed: {speedPercentage:0}%"; // Update the Text element
        speedDisplayText.enabled = true; // Ensure the text is visible
    }

    // New method to hide the speed display text
    void HideSpeedDisplay()
    {
        speedDisplayText.enabled = false; // Hide speed display
    }
}
