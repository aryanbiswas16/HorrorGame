using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSign : MonoBehaviour
{
    public GameObject gameOverSign;
    void Start()
    {
        gameOverSign.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
