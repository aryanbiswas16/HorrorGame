using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    private bool LightSwitch;
    [SerializeField]
    private GameObject _gameObject;

    private void OnMouseDown()
    {
        LightSwitch = !LightSwitch;
        if(LightSwitch)
        {
            TurnOffLight();
        }
        else
        {
            TurnOnLight();
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
}
