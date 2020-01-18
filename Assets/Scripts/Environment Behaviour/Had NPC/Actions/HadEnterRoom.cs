using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadEnterRoom : MonoBehaviour
{
    public float delayBeforeWalking = 1.5f;

    private float startWalkingTime;
    private bool startedWalking = false;

    void Start()
    {
        startWalkingTime = Time.time + delayBeforeWalking;
    }

    void Update()
    {
        if (!startedWalking && Time.time > startWalkingTime)
        {
            EventManager.TriggerEvent("move_had_in_front_of_wheelchair");
            startedWalking = true;
        }
    }
}
