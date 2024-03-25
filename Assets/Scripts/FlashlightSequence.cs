using System.Collections;
using UnityEngine;
using TMPro;



public class FlashlightSequence : MonoBehaviour
{
    public TextMeshProUGUI sequenceDisplay;
    public Flashlight flashlight;
    private KeyCode[] keySequence = { KeyCode.P, KeyCode.O, KeyCode.I };
    private int currentIndex = 0;
    private bool isSequenceActive = false;

    void Start()
    {
        sequenceDisplay.text = ""; // This will clear the text when the game starts
    }

    void Update()
    {
        if (flashlight.IsFlickering() && Input.GetKeyDown(KeyCode.G) && !isSequenceActive)
        {
            Debug.Log("G pressed");
            isSequenceActive = true;
            currentIndex = 0;
            ShowNextKeyInSequence();
        }

        if (isSequenceActive && currentIndex < keySequence.Length)
        {
            if (Input.GetKeyDown(keySequence[currentIndex]))
            //Debug.Log(isSequenceActive);
            //Debug.Log(currentIndex);
            Debug.Log("Key pressed");
            {
                currentIndex++;
                if (currentIndex >= keySequence.Length)
                {
                    StartCoroutine(SequenceCompleted());
                }
                else
                {
                    ShowNextKeyInSequence();
                }
            }
        }
    }

    public void OnFlickeringStarted()
    {
        sequenceDisplay.text = "Click G To Fix Flickering"; // Ensure this is the correct message
        currentIndex = 0; // Reset the sequence
        isSequenceActive = false; // The sequence can now be started with 'G'
    }

    private void ShowNextKeyInSequence()
    {
        // Only show the next key prompt if the sequence has started
        if (isSequenceActive)
        {
            sequenceDisplay.text = "Press " + keySequence[currentIndex].ToString() + " to fix the flashlight";
        }
    }

    private IEnumerator SequenceCompleted()
    {
        sequenceDisplay.text = "The Flashlight is Fixed";
        flashlight.StopFlicker();
        yield return new WaitForSeconds(1);
        sequenceDisplay.text = "";
        isSequenceActive = false;
    }
}






 