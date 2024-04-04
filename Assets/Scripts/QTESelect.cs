using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEHandler : MonoBehaviour
{
    // Array to store the QTE objects
    public GameObject[] qteObjects;

    private void Start()
    {
        // Start the coroutine that manages the QTE activation
        StartCoroutine(ActivateRandomQTE());
    }

    private IEnumerator ActivateRandomQTE()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(20f, 25f));

            // Deactivate all QTEs first
            foreach (var qte in qteObjects)
            {
                qte.SetActive(false);
            }

            // Randomly select one QTE to activate
            int randomIndex = Random.Range(0, qteObjects.Length);
            qteObjects[randomIndex].SetActive(true);
        }
    }
}
