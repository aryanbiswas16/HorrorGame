using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public MemorySequenceManager memorySequenceManager; // Reference to the MemorySequenceManager instead of QuizManager
    public GameObject player; // Assignable player GameObject

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the assigned player GameObject
        if (collision.gameObject == player)
        {
            // Make sure the sequence game is only setup and shown if it's currently not active
            if (!memorySequenceManager.sequencePanel.activeSelf)
            {   
                memorySequenceManager.StartSequence(); // Start the memory sequence game
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the exiting object is the assigned player GameObject
        if (collision.gameObject == player)
        {
            // Hide the sequence panel when the player leaves the collision area
            memorySequenceManager.sequencePanel.SetActive(false);
        }
    }
}
