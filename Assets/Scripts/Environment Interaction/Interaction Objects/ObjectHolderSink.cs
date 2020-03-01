using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolderSink : ObjectHolder
{

    public Pot pot;
    public PotSurfaceBubble potSurfaceBubble;
    public Faucet faucett;
    public WaterLevel waterLevel;
    public Vector3 heldObjectRotation;
    public float waterLevelInflowThreshold = 0.6596921f;
    public float potInflowRate = 0.0006f;

    public bool PotFilledByFaucet
    {
        get { return potFilledByFaucet; }
        set
        {
            if (value && !PotFilledByFaucet)
            {
                potFilledByFaucet = true;
                waterLevel.FilledByFaucet = false;
                potSurfaceBubble.PotIsInSkink = true;
            }
            else if (!value && PotFilledByFaucet)
            {
                potFilledByFaucet = false;
                waterLevel.FilledByFaucet = true;
                potSurfaceBubble.PotIsInSkink = false;
            }
        }
    }
    private bool potFilledByFaucet;


    void Update()
    {
        if (pot != null && PotFilledByFaucet)
        {
            if (!pot.TryAddWater(faucett.TotalWaterRateCuM * Time.deltaTime))
            {
                waterLevel.FilledByFaucet = true;
            }
            else
            {
                waterLevel.FilledByFaucet = false;
            }
            if (waterLevel.waterLevel > waterLevelInflowThreshold)
            {
                if (pot.TryAddWater(potInflowRate * Time.deltaTime))
                {
                    waterLevel.TryRemoveWater(potInflowRate * Time.deltaTime);
                }
            }
        }
    }

    public void TakeControlOfFaucet()
    {
        throw new NotImplementedException();
    }

    public void RelinquishControlOfFaucet()
    {
        throw new NotImplementedException();
    }
}
