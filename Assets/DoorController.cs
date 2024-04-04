using UnityEngine;

public class DoorController : MonoBehaviour
{
    public void OpenDoor()
    {
        // Move the door to the right to "open" it
        transform.position += Vector3.right * 10;
    }
}
