using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CupboardAudio : MonoBehaviour
{
    public AudioClip openTopDrawerClip;
    public AudioClip closeTopDrawerClip;
    public AudioClip openBottomDrawerClip;
    public AudioClip closeBottomDrawerClip;
    public AudioClip openCupboardDoorClip;
    public AudioClip closeCupboardDoorClip;

    public AudioClip dropRutabagaClip;
    public AudioClip dropCeleryClip;

    public AudioClip chopRutabagaInHalfClip;
    public AudioClip chopHalfRutabagIntoQuartersClip;
    public AudioClip chopQuarterRutabagaIntoPiecesClip;
    public AudioClip chopCeleryIntoPiecesClip;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        EventManager.StartListening("open_top_drawer", PlayOpenTopDrawer);
        EventManager.StartListening("close_top_drawer", PlayCloseTopDrawer);
        EventManager.StartListening("open_bottom_drawer", PlayOpenBottomDrawer);
        EventManager.StartListening("close_bottom_drawer", PlayCloseBottomDrawer);
        EventManager.StartListening("open_cupboard_door", PlayOpenCupboardDoor);
        EventManager.StartListening("close_cupboard_door", PlayCloseCupboardDoor);

        EventManager.StartListening("drop_rutabaga_on_surface", PlayDropRutagaba);
        EventManager.StartListening("drop_celery_on_surface", PlayDropCelery);
        EventManager.StartListening("chop_rutabaga_in_half", PlayChopRutagabaInHalf);
        EventManager.StartListening("chop_half_rutabaga_into_quarters", PlayChopHalfRutabagaIntoQuarters);
        EventManager.StartListening("chop_quarter_rutabaga_into_pieces", PlayChopQuarterRutabagaIntoPieces);
        EventManager.StartListening("chop_celery_into_pieces", PlayChopCeleryIntoPieces);
    }

    void PlayOpenTopDrawer()
    {
        audioSource.PlayOneShot(openTopDrawerClip);
    }

    void PlayCloseTopDrawer()
    {
        audioSource.PlayOneShot(closeTopDrawerClip);
    }

    void PlayOpenBottomDrawer()
    {
        audioSource.PlayOneShot(openBottomDrawerClip);
    }

    void PlayCloseBottomDrawer()
    {
        audioSource.PlayOneShot(closeBottomDrawerClip);
    }

    void PlayOpenCupboardDoor()
    {
        audioSource.PlayOneShot(openCupboardDoorClip);
    }

    void PlayCloseCupboardDoor()
    {
        audioSource.PlayOneShot(closeCupboardDoorClip, 0.5f);
    }

    void PlayDropRutagaba()
    {
        audioSource.PlayOneShot(dropRutabagaClip, 0.5f);
    }

    void PlayDropCelery()
    {
        audioSource.PlayOneShot(dropCeleryClip, 0.255f);
    }

    void PlayChopRutagabaInHalf()
    {
        audioSource.PlayOneShot(chopRutabagaInHalfClip);
    }

    void PlayChopHalfRutabagaIntoQuarters()
    {
        audioSource.PlayOneShot(chopHalfRutabagIntoQuartersClip);
    }

    void PlayChopQuarterRutabagaIntoPieces()
    {
        audioSource.PlayOneShot(chopQuarterRutabagaIntoPiecesClip);
    }

    void PlayChopCeleryIntoPieces()
    {
        audioSource.PlayOneShot(chopCeleryIntoPiecesClip);
    }
}
