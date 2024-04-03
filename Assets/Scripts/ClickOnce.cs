using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MutualDisableButtons : MonoBehaviour
{
    public Button button1; // Reference to the first button
    public Button button2; // Reference to the second button
    public GameObject qteSound;

    public GameObject Call;
    // Assume that there is a HealthManager script in your project that manages the player's health
    public HealthManager healthManager; 

    private void OnEnable()
    {
        button1.interactable = true;
        button2.interactable = true;
        qteSound.SetActive(true);

        button1.onClick.AddListener(ButtonClicked);
        button2.onClick.AddListener(ButtonClicked);

        // Start the coroutine for the QTE
        StartCoroutine(QTECoroutine());
    }

    private void ButtonClicked()
    {
        EndQTE();
    }

    private IEnumerator QTECoroutine()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);
        // If the QTE is still active after 5 seconds, end it and remove a heart
        if (button1.interactable)
        {
            EndQTE();
            // Assuming your HealthManager has a method called RemoveHeart
            if (healthManager != null)
            {
                healthManager.RemoveHeart();
                Call.SetActive(false);
            }
            else
            {
                Debug.LogError("HealthManager reference not set in MutualDisableButtons script.");
            }
        }
    }

    private void EndQTE()
    {
        // Disable both buttons, making them unclickable
        button1.interactable = false;
        button2.interactable = false;
        qteSound.SetActive(false); // Assuming this disables the QTE sound
        // Remove listeners if they shouldn't persist after the QTE ends
        button1.onClick.RemoveListener(ButtonClicked);
        button2.onClick.RemoveListener(ButtonClicked);
    }
}
