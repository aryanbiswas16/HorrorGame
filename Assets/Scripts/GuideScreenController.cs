using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GuideScreenController : MonoBehaviour
{
    public GameObject guideScreen; // Assign in inspector

    void Update()
    {
        // Check if ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Hide the guide screen
            guideScreen.SetActive(false);
        }
    }

    public void ShowGuideScreen()
    {
        // Show the guide screen
        guideScreen.SetActive(true);
    }
}
