using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPWalkingAudio : MonoBehaviour
{

    //public CharacterMotor characterMotor;

    public float minStepPauseDuration = 0.3f;
    public float maxStepPauseDuration = 0.5f;
    public AudioClip[] footstepAudio;

    private float nextStepTime = 0;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //if (characterMotor.IsGrounded())
        //{
        //    float averageVelocity = Mathf.Abs(
        //        (characterMotor.movement.velocity.x
        //        + characterMotor.movement.velocity.y
        //        + characterMotor.movement.velocity.z) 
        //        / 3
        //    );

        //    if (averageVelocity > 0.0f)
        //    {
        //        if (Time.time > nextStepTime)
        //        {
        //            audioSource.PlayOneShot(footstepAudio[Random.Range(0, footstepAudio.Length)]);
        //            nextStepTime = Time.time + Random.Range(minStepPauseDuration, maxStepPauseDuration);
        //        }
        //    }
        //}
    }
}
