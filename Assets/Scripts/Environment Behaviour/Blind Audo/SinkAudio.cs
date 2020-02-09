using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class SinkAudio : MonoBehaviour
{
    public AudioClip putDownPotClip;
    public float tapTranitionDuration = 2f;
    public float tapVolume = 0.5f;

    AudioSource audioSource;
    float tapTransitionFinishedTime = 0f;
    bool tapOn = false;
    bool tapTurning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        EventManager.StartListening("turn_on_tap", TurnOnTap);
        EventManager.StartListening("turn_off_tap", TurnOffTap);
        EventManager.StartListening("put_pot_in_sink", PutDownPot);
    }

    void Update()
    {
        if (tapTransitionFinishedTime > Time.time)
        {
            float tapTransitioned = (tapTransitionFinishedTime - Time.time) / tapTranitionDuration;
            if (tapOn)
            {
                audioSource.volume = tapTransitioned * tapVolume;
            }
            else
            {
                audioSource.volume = (1f - tapTransitioned) * tapVolume;
            }
        }
        else if (tapTurning)
        {
            tapOn = !tapOn;
            if (tapOn)
            {
                EventManager.TriggerEvent("tap_turned_on");
            }
            else
            {
                EventManager.TriggerEvent("tap_turned_off");
            }
            tapTurning = false;
        }
    }

    void TurnOnTap()
    {
        if (!tapOn)
        {
            tapTransitionFinishedTime = Time.time + tapTranitionDuration;
            tapTurning = true;
        }
        else
        {
            throw new InvalidOperationException("The tap is already on.");
        }
    }

    void TurnOffTap()
    {
        {
            if (tapOn)
            {
                tapTransitionFinishedTime = Time.time + tapTranitionDuration;
                tapTurning = true;
            }
            else
            {
                throw new InvalidOperationException("The tap is already off.");
            }
        }
    }

    void PutDownPot()
    {
        audioSource.PlayOneShot(putDownPotClip);
    }

}
