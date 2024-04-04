using UnityEngine;

public class DoorController : MonoBehaviour
{
    public AudioSource audioSource;
    public void OpenDoor()
    {
        // Move the door to the right to "open" it
        transform.position += Vector3.right * 10;
        audioSource.Play();
    }
}
