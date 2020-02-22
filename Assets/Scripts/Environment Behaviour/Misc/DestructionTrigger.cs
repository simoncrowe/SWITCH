using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject.Destroy(other.gameObject);
    }
}
