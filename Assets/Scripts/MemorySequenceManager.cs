using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MemorySequenceManager : MonoBehaviour
{   

    public GameObject sequencePanel; // Panel to show/hide the sequence UI
    public Button[] sequenceButtons; // Buttons to use for the sequence
    public DoorController doorController;
    public GameObject objectToDisable; // GameObject to disable on successful sequence replication
    public TMP_Text feedbackText; // Text to display feedback to the player

    private List<int> sequence = new List<int>(); // Stores the generated sequence
    private int currentSequenceIndex = 0; // Tracks the player's progress in replicating the sequence
    private int attemptCount = 0; // Tracks the number of attempts
    private const int maxAttempts = 3; // Maximum number of attempts

    // Colors for highlighting and unhighlighting the buttons
    public Color highlightColor = Color.yellow;
    public Color normalColor = Color.white; // Assumes buttons are white by default


    public Player Player; // Reference to the player's movement script

private void DisablePlayer()
{
    if (Player != null)
    {
        Player.canMove = false; // Disable movement
    }
}

private void EnablePlayer()
{
    if (Player != null)
    {
        Player.canMove = true; // Enable movement
    }
}
    private void Start()
    {   
     
        sequencePanel.SetActive(false);
        feedbackText.text = ""; // Ensure feedback text is clear at start
    }

public void StartSequence()
{
    sequence.Clear();
    currentSequenceIndex = 0;
    attemptCount = 0; // Reset attempts
    feedbackText.text = ""; // Clear feedback text
    DisablePlayer(); // Disable player movement when sequence starts
    GenerateSequence(6); // Generates a sequence of 6 steps
    StartCoroutine(PlaySequence());
}

    void GenerateSequence(int length)
    {
        for (int i = 0; i < length; i++)
        {
            sequence.Add(Random.Range(0, sequenceButtons.Length));
        }
    }

    IEnumerator PlaySequence()
    {
        sequencePanel.SetActive(true);
        foreach (var buttonIndex in sequence)
        {
            HighlightButton(buttonIndex);
            yield return new WaitForSeconds(1f);
            UnhighlightAllButtons();
            yield return new WaitForSeconds(0.5f);
        }

        EnableButtonInteractions();
    }

    void HighlightButton(int index)
    {
        var buttonImage = sequenceButtons[index].GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = highlightColor;
        }
    }

    void UnhighlightAllButtons()
    {
        foreach (var button in sequenceButtons)
        {
            var buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = normalColor;
            }
        }
    }

    void EnableButtonInteractions()
    {
        foreach (var button in sequenceButtons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => ButtonPressed(System.Array.IndexOf(sequenceButtons, button)));
        }
    }

    void ButtonPressed(int index)
{
    if (sequencePanel.activeSelf && currentSequenceIndex < sequence.Count)
    {
        if (index == sequence[currentSequenceIndex])
        {
            currentSequenceIndex++;
            if (currentSequenceIndex >= sequence.Count)
            {
                Debug.Log("Sequence successfully replicated!");
                doorController.OpenDoor();
                objectToDisable.SetActive(false);
                sequencePanel.SetActive(false);
                feedbackText.text = "Correct!";
                EnablePlayer(); // Re-enable movement on success

            }
        }
        else
        {
            attemptCount++;
            if (attemptCount < maxAttempts)
            {
                feedbackText.text = "WRONG! Attempts left: " + (maxAttempts - attemptCount);
                currentSequenceIndex = 0; // Reset sequence index for retry
            }
            else
            {
                feedbackText.text = "FAILED! No more attempts.";
                Debug.Log("Failed all attempts.");
                sequencePanel.SetActive(false);
                EnablePlayer(); // Re-enable movement after all attempts are failed
            }
        }
    }
}
}