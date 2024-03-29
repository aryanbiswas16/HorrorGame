using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this if using TextMeshPro

public class Flashlight : MonoBehaviour
{
    private bool LightSwitch;
    [SerializeField]
    private GameObject _gameObject;

    public float batteryLife = 5f; // Total battery life in seconds
    private float currentBatteryLife;
    private bool isFlashlightOn = false;

    // Reference to the UI Text or TextMeshPro component
    public TextMeshProUGUI batteryLifeText; // For TextMeshPro
    // public Text batteryLifeText; // For standard UI Text

    void Start()
    {
        currentBatteryLife = batteryLife;
        _gameObject.SetActive(false);
        UpdateBatteryLifeUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentBatteryLife > 0 || isFlashlightOn)
            {
                ToggleFlashlight();
            }
        }

        if (isFlashlightOn && currentBatteryLife > 0)
        {
            currentBatteryLife -= Time.deltaTime;
            UpdateBatteryLifeUI();

            if (currentBatteryLife <= 0)
            {
                currentBatteryLife = 0;
                TurnOffLight();
            }
        }
    }

    void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;

        if (isFlashlightOn)
        {
            TurnOnLight();
        }
        else
        {
            TurnOffLight();
        }
    }

    void TurnOnLight()
    {
        _gameObject.SetActive(true);
    }

    void TurnOffLight()
    {
        _gameObject.SetActive(false);
    }

    // Update the UI with the current battery life
    void UpdateBatteryLifeUI()
    {
        if (batteryLifeText != null)
        {
            batteryLifeText.text = "Battery: " + Mathf.CeilToInt(currentBatteryLife).ToString() + "%";
        }
    }
}
