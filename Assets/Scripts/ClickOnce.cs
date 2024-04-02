using UnityEngine;
using UnityEngine.UI;

public class MutualDisableButtons : MonoBehaviour
{
    public Button button1; // Reference to the first button
    public Button button2; // Reference to the second button

    public GameObject qteSound;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        button1.interactable = true;
        button2.interactable = true;
        qteSound.SetActive(true); // Make sure qteSound is a GameObject reference as previously corrected

        // Add a click listener to each button, which will call the ButtonClicked method when either button is clicked
        button1.onClick.AddListener(ButtonClicked);
        button2.onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked()
    {
        // Disable both buttons, making them unclickable
        button1.interactable = false;
        button2.interactable = false;
    }
}