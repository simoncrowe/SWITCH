using UnityEngine;
using System.Collections;

public class Faucet : MonoBehaviour
{
    private ParticleSystem waterParticleSys;
    private AudioSource audioSource;

    public float baseTapOutputPerSecond = 0.00005f;
    // Output in cubic meters for the machines!
    public float TotalWaterRateCuM
    {
        get { return TotalWaterRate * baseTapOutputPerSecond; }
    }
    public float TotalWaterRate { get; private set; }
    public Tap coldTap;
    public Tap hotTap;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        waterParticleSys = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        TotalWaterRate = coldTap.state + hotTap.state + 0.04f;
        audioSource.volume = (TotalWaterRate / 2f) + 0.04f;
        var waterParticleEmission = waterParticleSys.emission;
        var waterParticleSysShape = waterParticleSys.shape;
        var waterParticleSysMain = waterParticleSys.main;

        if (TotalWaterRate > 0.01f)
        {
            waterParticleEmission.rateOverTime = 400f + (200f * TotalWaterRate);
            waterParticleSysShape.radius = 0.01f + (TotalWaterRate / 19);
            waterParticleSysMain.startSize = .029f * TotalWaterRate;
        }
        else
        {
            waterParticleEmission.rateOverTime = 0.1f * TotalWaterRate;
            waterParticleSysShape.radius = 0.01f;
            waterParticleSysMain.startSize = .001f * TotalWaterRate;
        }
    }
}
