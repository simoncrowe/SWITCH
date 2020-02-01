using UnityEngine;

public class PotAudio : MonoBehaviour
{
    AudioSource audioSource;
    float boilDuration = 30;
    float boilStart;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        EventManager.StartListening("cook_stew", StartBoilingSequence);
    }

    void Update()
    {
        float boilElapsed = Time.time - boilStart;
        if (boilElapsed < boilDuration)
        {
            audioSource.volume = 1 - (boilElapsed / boilDuration);
        }
        else
        {
            audioSource.volume = 0;
        }
    }

    void StartBoilingSequence()
    {
        boilStart = Time.time;
    }
}
