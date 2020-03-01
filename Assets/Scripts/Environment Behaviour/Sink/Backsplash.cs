using UnityEngine;
using System.Collections;

public class Backsplash : MonoBehaviour
{

    public Faucet faucet;
    public ObjectHolderSink potHolder;
    private ParticleSystem particleSys;

    void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var particleEmission = particleSys.emission;
        var particleSysMain = particleSys.main;

        if (!potHolder.PotFilledByFaucet && faucet.TotalWaterRate > 0)
        {
            particleEmission.enabled = true;
            particleEmission.rateOverTime = 90f  *  faucet.TotalWaterRate;
            particleSysMain.startSize = 0.02f  *  faucet.TotalWaterRate;
            particleSysMain.startSpeed = 0.3f  *  faucet.TotalWaterRate;
        }
        else
        {
            particleEmission.enabled = false;
        }
    }
}
