using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    public GameObject textGameObject; // Assign the GameObject that contains your Text component
    public Button yourButton; // Assign your Button element in the inspector

    private Coroutine showTextCoroutine; // To keep track of the coroutine

    void Start()
    {
        textGameObject.SetActive(false); // Ensure the text is initially inactive

        // Add a listener to your button which will trigger the ShowTextForSeconds method
        yourButton.onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked()
    {
        // If there's already a coroutine running, stop it
        if (showTextCoroutine != null)
        {
            StopCoroutine(showTextCoroutine);
        }
        
        // Start the coroutine and store a reference to it
        showTextCoroutine = StartCoroutine(ShowTextForSeconds(3));
    }

    IEnumerator ShowTextForSeconds(float seconds)
    {
        textGameObject.SetActive(true); // Activate the text GameObject
        yield return new WaitForSeconds(seconds); // Wait for the specified amount of seconds
        textGameObject.SetActive(false); // Deactivate the text GameObject
    }
}
