using UnityEngine;

public class HadEnterRoom : MonoBehaviour
{
    public float delayBeforeWalking = 10f;


    void Start()
    {
        Invoke("StartWalking", delayBeforeWalking);
    }

    void StartWalking()
    {
        EventManager.TriggerEvent("move_had_in_front_of_wheelchair");
    }
}
