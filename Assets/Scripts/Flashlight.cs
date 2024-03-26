using System.Collections;
using UnityEngine;
using TMPro;

public class Flashlight : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameObject; // The light GameObject

    [SerializeField]
    private FlashlightSequence flashlightSequence; // Add this line

    public float batteryLifeInSeconds = 100f; // Adjust to fit the equivalent of 30% starting charge
    private float currentBatteryLife;
    public bool isFlashlightOn = false;

    public TextMeshProUGUI batteryLifeText; // Reference to TextMeshPro UI component for battery life

    private Coroutine flickerRoutine; // For tracking the flicker coroutine

    public bool flickering = false;

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
            //flickerRoutine = StartCoroutine(FlickerEffect());
            StartFlickering();
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
    // Ensure the light is on for logic checks, but visually flickers
    isFlashlightOn = true; // The light is logically on, even if visually flickering
    bool initiallyOn = _gameObject.activeSelf; // Remember the initial state

    // Turn on the light before flickering for visual consistency
    _gameObject.SetActive(true);

    while (true) // Continue flickering indefinitely
    {
        // Flicker the light by turning it off and on visually
        _gameObject.SetActive(!_gameObject.activeSelf);

        // Still deplete battery during flicker
        if (initiallyOn)
        {
            DepleteBattery(Time.deltaTime * 2); // Deplete faster during flicker if it was initially on
            if (currentBatteryLife <= 0) // Stop flickering if the battery dies
            {
                StopFlicker();
                yield break;
            }
        }

        yield return new WaitForSeconds(0.1f); // Control the flicker speed
    }
}

    public void StopFlicker()
    {
        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
            flickerRoutine = null;
            flickering = false; // Update flickering state
            // Restore the initial state of the flashlight
            _gameObject.SetActive(isFlashlightOn);
        }
    }



    void TurnOffLight()
    {
        isFlashlightOn = false;
        _gameObject.SetActive(false);
    }

    //public bool IsFlickering() {
    //    return flickerRoutine != null; // If flickerRoutine is active, the flashlight is flickering
    //}

    public bool IsFlickering() => flickering;

    //public void StartFlickering()
    //{
    //    if (flickerRoutine == null)
    //    {
    //        flickerRoutine = StartCoroutine(FlickerEffect());
            // Assuming flashlightSequence is a reference to the FlashlightSequence script
    //        flashlightSequence.OnFlickeringStarted();
    //    }
    //}

    public void StartFlickering()
    {
        if (!flickering)
        {
            flickering = true;
            flickerRoutine = StartCoroutine(FlickerEffect());
            // Assuming flashlightSequence is a reference to the FlashlightSequence script
            flashlightSequence.OnFlickeringStarted();
        }
    }


}