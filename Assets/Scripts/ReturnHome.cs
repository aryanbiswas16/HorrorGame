using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnHome : MonoBehaviour
{
    public void LoadHomeScreen()
    {
        Time.timeScale = 1;
        // Replace "HomeScreen" with the actual name of your home screen scene
        SceneManager.LoadScene("StartMenu");
    }
}
