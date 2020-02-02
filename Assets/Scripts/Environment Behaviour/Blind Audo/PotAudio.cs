using UnityEngine;

public class PotAudio : MonoBehaviour
{
    public AudioClip dropDicedRutabagaClip;
    public AudioClip dropChoppedCeleryClip;
    public AudioClip dropCornflourClip;

    AudioSource audioSource;
    float boilDuration = 30;
    float boilStart;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        EventManager.StartListening("play_pot_boiling_sound", StartBoilingSequence);
        EventManager.StartListening("play_drop_diced_rutabaga_in_pot_sound", DropDicedRutabagInEmptyPot);
        EventManager.StartListening("play_drop_chopped_celery_in_pot_sound", DropChoppedCeleryInEmptyPot);
        EventManager.StartListening("play_drop_cornflour_in_pot_sound", DropChoppedCeleryInEmptyPot);
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

    void DropDicedRutabagInEmptyPot()
    {
        audioSource.PlayOneShot(dropDicedRutabagaClip, 0.4f);
    }

    void DropChoppedCeleryInEmptyPot()
    {
        audioSource.PlayOneShot(dropChoppedCeleryClip, 0.2f);
    }

    void DropCornflourInEmptyPot()
    {
        audioSource.PlayOneShot(dropCornflourClip, 0.5f);
    }
}
