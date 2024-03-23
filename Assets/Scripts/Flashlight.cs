using System.Collections;
using UnityEngine;
using TMPro;

public class Flashlight : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameObject; // The light GameObject

    public float batteryLifeInSeconds = 100f; // Adjust to fit the equivalent of 30% starting charge
    private float currentBatteryLife;
    public bool isFlashlightOn = false;

    public TextMeshProUGUI batteryLifeText; // Reference to TextMeshPro UI component for battery life

    private Coroutine flickerRoutine; // For tracking the flicker coroutine

    void Start()
    {
        // Assuming 100 seconds of battery life equals 30% charge display
        currentBatteryLife = batteryLifeInSeconds;
        _gameObject.SetActive(false);
        UpdateBatteryLifeUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Allow toggling the flashlight unless flickering is active
            if (flickerRoutine == null && currentBatteryLife > 0)
            {
                ToggleFlashlight();
            }
        }

        if (isFlashlightOn)
        {
            // Deplete battery at the normal rate
            DepleteBattery(Time.deltaTime);
        }

        // Start flickering without affecting the flashlight state, unless the battery is dead
        if (Input.GetKeyDown(KeyCode.F) && flickerRoutine == null && currentBatteryLife > 0)
        {
            flickerRoutine = StartCoroutine(FlickerEffect());
        }

        // Stop flickering with G
        if (Input.GetKeyDown(KeyCode.G) && flickerRoutine != null)
        {
            StopFlicker();
        }
    }

    void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;
        _gameObject.SetActive(isFlashlightOn);
    }

    void DepleteBattery(float amount)
    {
        currentBatteryLife -= amount;
        if (currentBatteryLife <= 0)
        {
            currentBatteryLife = 0;
            TurnOffLight();
        }
        UpdateBatteryLifeUI();
    }

    void UpdateBatteryLifeUI()
    {
        if (batteryLifeText != null)
        {
            float batteryPercentage = (currentBatteryLife / batteryLifeInSeconds) * 100; // Assuming 100 seconds total equals 100% display
            batteryLifeText.text = Mathf.RoundToInt(batteryPercentage).ToString() + "%";
        }
    }

    IEnumerator FlickerEffect()
    {
        float singleFlickerDuration = 0.1f;
        bool initiallyOn = isFlashlightOn;

        while (true) // Continue flickering indefinitely
        {
            // Only deplete battery faster if the light was initially on
            if (initiallyOn)
            {
                DepleteBattery(Time.deltaTime * 2); // Example: deplete slightly faster during flicker
                if (currentBatteryLife <= 0) // Stop flickering if the battery dies
                {
                    StopFlicker();
                    yield break;
                }
            }

            _gameObject.SetActive(!_gameObject.activeSelf); // Toggle light for flicker
            yield return new WaitForSeconds(singleFlickerDuration);
        }
    }

    public void StopFlicker()
    {
        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
            flickerRoutine = null;
            _gameObject.SetActive(isFlashlightOn); // Restore light to its correct state
        }   
    }


    void TurnOffLight()
    {
        isFlashlightOn = false;
        _gameObject.SetActive(false);
    }
}
