using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour
{
    public void QuitGame()
    {
        // Log message to console (useful for debugging)
        Debug.Log("Quit game request");
        
        // Quit the application
        Application.Quit();

        // If running in the Unity Editor
        #if UNITY_EDITOR
            // Stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
