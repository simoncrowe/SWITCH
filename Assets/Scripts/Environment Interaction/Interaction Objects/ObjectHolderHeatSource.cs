using UnityEngine;
using System.Collections;


public class ObjectHolderHeatSource : ObjectHolder
{
    public float ThermalOutput { get { return baseThermalOutput * ThermalOutputMultiplier; } }  // Watt
    public float ThermalOutputMultiplier { get; set; }

    public float baseThermalOutput = 4600;  // Watt
}
