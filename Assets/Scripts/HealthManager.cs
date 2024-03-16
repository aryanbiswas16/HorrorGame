using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int health = 3;

    public Image[] hearts;

    void Awake(){
        health = 3;
    }

    // Update is called once per frame
    void Update()
    {
        // Loop through all hearts
        for (int i = 0; i < hearts.Length; i++)
        {
            // If the current index is less than the health value, the heart should be visible
            if (i < health)
            {
                hearts[i].enabled = true;
            }
            else
            {
                // If the current index is greater than or equal to the health value, disable the heart
                hearts[i].enabled = false;
            }
        }
    }
}