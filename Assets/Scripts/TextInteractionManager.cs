using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextInteractionManager : MonoBehaviour
{
    public GameObject messengerPanel; // The panel that shows the text messages
    public TextMeshProUGUI messageText; // Text component to display the wife's message
    public List<Button> responseButtons; // Buttons for player responses
    private int currentMessageIndex = 0;

    [System.Serializable]
    public class TextMessage
    {
        public string message;
        public string[] responses;
        public int correctResponseIndex;
    }

    public List<TextMessage> conversation; // The whole conversation

    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.H))
        {
            InitiateConversation();
        }
    }


    void Start()
    {
        messengerPanel.SetActive(false); // Hide the messenger panel initially
    }

    void Awake()
{
    // Sample conversation data
    conversation = new List<TextMessage>()
    {
        new TextMessage()
        {
            message = "Are you still at work?",
            responses = new string[] { "Just finishing up!", "No, I left already.", "It's been a long day." },
            correctResponseIndex = 0
        },
        // Add more messages as needed...
    };
}


    // Call this to start the conversation
    public void InitiateConversation()
    {
        if (conversation.Count > 0)
        {
            currentMessageIndex = 0;
            DisplayCurrentMessage();
        }
    }

    void DisplayCurrentMessage()
    {
        TextMessage currentMessage = conversation[currentMessageIndex];
        messageText.text = currentMessage.message; // Display the wife's message
        messengerPanel.SetActive(true); // Show the messenger panel

        // Set up response buttons
        for (int i = 0; i < responseButtons.Count; i++)
        {
            if (i < currentMessage.responses.Length)
            {
                responseButtons[i].gameObject.SetActive(true);
                responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentMessage.responses[i];
                int responseIndex = i;
                responseButtons[i].onClick.RemoveAllListeners();
                responseButtons[i].onClick.AddListener(() => Respond(responseIndex));
            }
            else
            {
                responseButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void Respond(int responseIndex)
    {
        if (responseIndex == conversation[currentMessageIndex].correctResponseIndex)
        {
            Debug.Log("Correct response selected.");
        }
        else
        {
            Debug.Log("Incorrect response selected.");
        }

        currentMessageIndex++;
        if (currentMessageIndex < conversation.Count)
        {
            DisplayCurrentMessage();
        }
        else
        {
            EndConversation();
        }
    }

    void EndConversation()
    {
        messengerPanel.SetActive(false);
        Debug.Log("End of the conversation.");
        // You can trigger any other game event from here.
    }
}
