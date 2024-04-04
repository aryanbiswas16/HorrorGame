using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public GameObject quizPanel;
    public TMP_Text questionText;
    public Button[] answerButtons;
    private int correctAnswerIndex;
    public DoorController doorController;
    public GameObject objectToDisable; // GameObject to disable on correct answer (e.g., the door or trigger)

    private int lastQuestionIndex = -1; // Keep track of the last question asked

    // Sample questions and answers
    private string[,] questions = new string[,]
    {
        { "What is 2 + 2?", "3", "4", "5", "6" },
        { "What is the capital of France?", "Paris", "London", "Berlin", "Madrid"}
    };

    // Indices of the correct answers (assuming 0-based indexing for your answers)
    private int[] correctAnswers = new int[] { 1, 0 }; // Correct answers for the provided questions

    private void Start()
    {
        quizPanel.SetActive(false);
    }

    public void SetupQuestion()
    {
        int questionIndex = GetNextQuestionIndex();
        questionText.text = questions[questionIndex, 0];
        correctAnswerIndex = correctAnswers[questionIndex]; // Use the correct answer index

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Local copy for lambda capture
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = questions[questionIndex, i + 1];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => AnswerSelected(index));
        }

        quizPanel.SetActive(true);
    }

    void AnswerSelected(int index)
    {
        if (index == correctAnswerIndex)
        {
            Debug.Log("Correct Answer!");
            doorController.OpenDoor();
            objectToDisable.SetActive(false); // Disable the specified GameObject
        }
        else
        {
            Debug.Log("Wrong Answer. Try again!");
            // The question will automatically switch for the next interaction
            quizPanel.SetActive(false);
        }
    }

    int GetNextQuestionIndex()
    {
        int questionIndex = Random.Range(0, questions.GetLength(0));
        // Ensure we don't repeat the last question
        while (questionIndex == lastQuestionIndex)
        {
            questionIndex = Random.Range(0, questions.GetLength(0));
        }
        lastQuestionIndex = questionIndex;
        return questionIndex;
    }
}
