using UnityEngine;
using System.Collections;

public class PotSteamParticle : MonoBehaviour
{
    public Pot pot;
    public int baseEmissionRate = 10;
    public float potTempratureEmissionWeight = 0.5f;
    ParticleSystem.EmissionModule steamEmmissionModule;

    void Start()
    {
        steamEmmissionModule = GetComponent<ParticleSystem>().emission;
    }

    void Update()
    {
        if (pot.temprature > pot.waterSimmeringPoint && pot.occupiedVolume > 0)
        {
            if (!steamEmmissionModule.enabled) { steamEmmissionModule.enabled = true; }
            steamEmmissionModule.rate = new ParticleSystem.MinMaxCurve
                (Mathf.Lerp(baseEmissionRate,
                baseEmissionRate * ((pot.temprature - pot.waterSimmeringPoint) / (pot.waterBoilingPoint - pot.waterSimmeringPoint)),
                potTempratureEmissionWeight));
        }
        else
        {
            steamEmmissionModule.enabled = false;
        }
    }
}