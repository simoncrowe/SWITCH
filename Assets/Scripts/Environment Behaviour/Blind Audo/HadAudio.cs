using UnityEngine;
using System.Collections;

public class HadAudio : MonoBehaviour
{
    public AudioClip eatFromPotClip;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        EventManager.StartListening("eat_from_pot", PlayEatFromPot);
    }

    void PlayEatFromPot()
    {
        audioSource.PlayOneShot(eatFromPotClip);
    }
}
