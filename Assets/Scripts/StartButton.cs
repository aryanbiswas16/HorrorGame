using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public GameObject storyboardPanel; // Drag your storyboard panel to this field in the Inspector.

    public void OnStartClicked()
    {
        storyboardPanel.SetActive(true); // Activate the storyboard panel when Start is clicked.
    }
}
