using UnityEngine;

public class DamageDealer : MonoBehaviour
{
private void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log("Triggered with: " + other.name);
    if (other.CompareTag("Boss"))
    {
        Debug.Log("Boss hit!");
        Destroy(gameObject);
    }
}

}