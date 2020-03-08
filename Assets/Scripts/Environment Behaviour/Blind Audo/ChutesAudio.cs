using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ChutesAudio : MonoBehaviour
{
    public float delayBeforeObjectsFalling = 2f;
    public AudioClip objectsFallClip;

    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Invoke("PlayObjectsFallSound", 3f);
    }

    void PlayObjectsFallSound()
    {
        audioSource.PlayOneShot(objectsFallClip);
    }
}
