using UnityEngine;
using System.Collections;


public class HobControl : InteractionObject
{

    public int knobState = 0;
    public float magnitude = 1.0f;
    public float ignitionInterval = 0.05f;
    public int ignitionStageCount = 14;
    public BitArray ignitionStages;

    private bool isIgniting = false;
    private int ignitionStageCounter = 0;
    private float nextIgnite;
    public StoveAudioR hobAudio;
    public ObjectHolderHeatSource hobHeatSource;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ignitionStages = new BitArray(ignitionStageCount + 1);
    }

    void Update()
    {
        if (IsSelected && Input.GetButtonDown("Interact"))
        {
            audioSource.Play();
            knobState++;
            knobState = knobState % 7;
            if (knobState == 0)
            {
                ignitionStages.SetAll(false);
                magnitude = 0;
            }
            else if (knobState == 1)
            {
                magnitude = 1.0f;
                isIgniting = true;
            }
            else if (knobState > 1)
            {
                magnitude = 1.0f + (0.15f * knobState);
            }
            transform.eulerAngles = new Vector3(0f, 51.428571f * knobState, 0f);
            hobAudio.ChangeState(knobState);
            hobHeatSource.ThermalOutputMultiplier = magnitude / 2;
        }

        if (isIgniting)
        {
            Ignite();
        }
    }
    void Ignite()
    {
        if (nextIgnite < Time.time)
        {
            ignitionStages.Set(ignitionStageCounter, true);
            ignitionStageCounter++;
            nextIgnite = Time.time + ignitionInterval;

            if (ignitionStageCounter + 1 == ignitionStages.Count)
            {
                isIgniting = false;
                ignitionStageCounter = 0;

            }

        }

    }
}
