using UnityEngine;
using System.Collections;

public class StoveAudio : MonoBehaviour
{
    public AudioClip putDownPotClip;
    public AudioClip turnOnHobClip;
    public AudioClip knobCickClip;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;

        EventManager.StartListening("put_pot_on_stove", PutPotOnStove);
        EventManager.StartListening("turn_on_hob", TurnOnHob);
        EventManager.StartListening("turn_off_hob", TurnOffHob);
    }

    void PutPotOnStove()
    {
        audioSource.PlayOneShot(putDownPotClip);
    }

    void TurnOnHob()
    {
        audioSource.PlayOneShot(turnOnHobClip);
        Invoke("StartPlayingHobLoop", 0.25f);
    }

    void TurnOffHob()
    {
        audioSource.PlayOneShot(knobCickClip);
        StopPlayingHobLoop();
    }

    void StartPlayingHobLoop()
    {
        audioSource.volume = 0.1f;
        Invoke("StartPlayingHobLoopLoud", 0.5f);
    }

    void StartPlayingHobLoopLoud()
    {
        audioSource.volume = 0.2f;
    }

    void StopPlayingHobLoop()
    {
        audioSource.volume = 0f;
    }
}
