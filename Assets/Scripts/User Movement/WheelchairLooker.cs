using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelchairLooker : MonoBehaviour
{
    public GameObject wheelchair;
    public FirstPersonController firstPersonController;

    void Start()
    {
        EventManager.StartListening("had_look_at_wheelchair", LookAtWheelchair);
    }

    void LookAtWheelchair()
    {
        // Update MouseLook's internal rotation state
        transform.LookAt(wheelchair.transform.position +  new Vector3(0, 0.5f, 0));
        firstPersonController.m_MouseLook.Init(
             firstPersonController.gameObject.transform,
             transform
        );
    }

}
