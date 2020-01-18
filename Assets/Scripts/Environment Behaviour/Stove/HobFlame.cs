using UnityEngine;

public class HobFlame : MonoBehaviour { 

    public HobControl hobControl;
    public int ignitionStageNumber;
    public float particleSpeedMultiplier = .25f;
    public float particleSizeMultiplier = .013f;

    private ParticleSystem particleSys;
    private float magnitude;

    void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var particleEmission = particleSys.emission;

        if (hobControl.ignitionStages[ignitionStageNumber] && !particleEmission.enabled)
        {
            particleEmission.enabled = true;

        }
        else if (particleEmission.enabled)
        {
            particleEmission.enabled = false;
        }

        if (magnitude != hobControl.magnitude)
        {
            magnitude = hobControl.magnitude;

            var particleVelocity = particleSys.velocityOverLifetime;
            particleVelocity.speedModifier = particleSpeedMultiplier * magnitude;

            var particleSysMain = particleSys.main;
            particleSysMain.startSize = particleSizeMultiplier * magnitude;
        }
    }
}
