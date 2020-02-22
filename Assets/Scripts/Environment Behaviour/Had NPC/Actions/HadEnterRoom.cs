using UnityEngine;

public class HadEnterRoom : MonoBehaviour
{
    public float delayBeforeWalking = 10f;


    void Start()
    {
        // Resetting cursor state from scene 0
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Invoke("StartWalking", delayBeforeWalking);
    }

    void StartWalking()
    {
        EventManager.TriggerEvent("move_had_in_front_of_wheelchair");
    }
}
