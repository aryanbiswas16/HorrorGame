using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public QuizManager quizManager;
    public GameObject player; // Assignable player GameObject

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the assigned player GameObject
        if (collision.gameObject == player)
        {
            // Make sure the quiz is only setup and shown if it's currently not active
            if (!quizManager.quizPanel.activeSelf)
            {
                quizManager.SetupQuestion();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the exiting object is the assigned player GameObject
        if (collision.gameObject == player)
        {
            // Hide the quiz panel when the player leaves the collision area
            quizManager.quizPanel.SetActive(false);
        }
    }
}