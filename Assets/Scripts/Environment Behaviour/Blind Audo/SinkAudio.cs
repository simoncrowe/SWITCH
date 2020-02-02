using UnityEngine;
using System.Collections;

public class NewMonoBehaviour : MonoBehaviour
{
    public AudioClip waterPourSound;

    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }
}
