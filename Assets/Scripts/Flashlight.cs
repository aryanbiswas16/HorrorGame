using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this if using TextMeshPro

public class Flashlight : MonoBehaviour
{
    private bool LightSwitch;
    [SerializeField] private GameObject _gameObject;


    public float batteryLife = 5f; // Total battery life in seconds
    private float currentBatteryLife;
    public bool isFlashlightOn = false;

 
    private SoundFXManager soundFXManager;

    // Reference to the UI Text or TextMeshPro component
    public TextMeshProUGUI batteryLifeText; // For TextMeshPro
    // public Text batteryLifeText; // For standard UI Text

    void Start()
    {
        soundFXManager = SoundFXManager.GetInstance();
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
        AudioClip switchSound = Resources.Load<AudioClip>("Sounds/Clips/Light-Switch");
        soundFXManager.Play(switchSound);

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

    void UpdateBatteryLifeUI()
    {
        if (batteryLifeText != null)
        {
            float batteryPercentage = (currentBatteryLife / batteryLife) * 30;
            batteryLifeText.text = Mathf.RoundToInt(batteryPercentage).ToString() + "%";
        }
    }

    // Public method to increase battery life by 5 seconds
    public void IncreaseBatteryLife()
    {
        currentBatteryLife = Mathf.Min(currentBatteryLife + 5, batteryLife);
        UpdateBatteryLifeUI(); // Update the UI to reflect the new battery life
    }
}
