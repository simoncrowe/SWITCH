using UnityEngine;
using System.Collections;

public class HadAudio : MonoBehaviour
{
    public AudioClip eatFromPotClip;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        EventManager.StartListening("play_eat_from_pot_sound", PlayEatFromPot);
    }

    void PlayEatFromPot()
    {
        audioSource.PlayOneShot(eatFromPotClip);
    }
}
