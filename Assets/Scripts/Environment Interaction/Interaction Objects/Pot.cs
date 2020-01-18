using UnityEngine;
using System.Collections.Generic;
using System;


public class Pot : Food
{
    public float Temprature { get; private set; }     // Kelvin

    public float waterSimmeringPoint = 350f;
    public float waterBoilingPoint = 373.15f;
    public float waterCoolingRate = 5f;
    public float secondaryWaterCoolingRate = 2f;
    public float waterRoomTemprature = 269f;
    public float capacity = 0.001337f; // Cubic meters
    public float occupiedVolume = 0.001113285f;   // Cubic metres
    public float ambientTemprature = 273f;       // Kelvins
    public float hobPotEnergyEfficiency = 0.16f; // Would need some sort of table/array of efficincies for multiple possibilities
    public float contentsCookingRate = 0.01f;

    public ObjectHolderSink potHolderSink;
    public Transform surfaceTransform;
    public MeshRenderer surfaceRenderer;
    public Color clearSurfaceColor;
    public Vector3 surfacePositionEmpty;
    public Vector3 surfacePositionFull;
    public Vector3 surfaceScaleEmpty;
    public Vector3 surfaceScaleFull;

    public Color cornflourSurfaceColor;
    public Color burntSurfaceColor;
    public GameObject celeryPrefabObject;
    public GameObject rutabagaPrefabObject;
    public Vector3 rutabagaOffset;
    public Vector3 celeryOffset;

    private Color surfaceColour;
    private GameObject celeryGameObject;
    private GameObject rutabagaGameObject;
    private bool containsCornflour = false;
    private bool containsCelery = false;
    private bool containsRutabaga = false;
    private bool halfEaten = false;
    private float contentsCookedness = 0;

    public override void InteractantHasPickedUp()
    {
        potHolderSink.PotFilledByFaucet = false;
    }

    // This provides an adequate estimate as the volume is incremented when ingredients are added
    public override float ObjectVolume
    {
        get
        {
            return occupiedVolume + GetSolidIngredientVolume();
        }
    }

    public override void InteractantHasConsumedHalfContents()
    {
        throw new NotImplementedException();
    }

    public override void InteractantHasConsumedAllContents()
    {
        throw new NotImplementedException();
    }

    float GetSolidIngredientVolume()
    {
        float solidIngredientVolume = 0;
        if (containsCelery) { solidIngredientVolume += celeryGameObject.GetComponent<ChoppedCelery>().ObjectVolume; }
        if (containsRutabaga) { solidIngredientVolume += rutabagaGameObject.GetComponent<DicedRutabaga>().ObjectVolume; }
        return solidIngredientVolume;
    }

    void Start()
    {
        Temprature = waterRoomTemprature;
        surfaceColour = surfaceRenderer.material.color;
    }

    void Update()
    {
        if (holdableObjectState == HoldableObjectStates.HeldByObjectHolder)
        {
            if (mostRecentObjectHolder is ObjectHolderHeatSource)
            {
                ObjectHolderHeatSource currentHeatSource = mostRecentObjectHolder as ObjectHolderHeatSource;
                if (currentHeatSource.ThermalOutput > 0)
                {
                    if (Temprature < waterBoilingPoint)
                    {
                        // Heat transfer rate (W) * temprature gain resulting from heating contents of pot by 1 Jouls * duration of previous frame
                        Temprature += (currentHeatSource.ThermalOutput * (0.239f / (occupiedVolume * 1e6f))) * Time.deltaTime;
                    }
                }
                else
                {
                    if (Temprature > waterSimmeringPoint)
                    {
                        Temprature -= waterCoolingRate * Time.deltaTime;
                    }
                    else if (Temprature > waterRoomTemprature)
                    {
                        Temprature -= secondaryWaterCoolingRate * Time.deltaTime;
                    }
                }
            }
        }
        else
        {
            if (Temprature > waterSimmeringPoint)
            {
                Temprature -= waterCoolingRate * Time.deltaTime;
            }
            else if (Temprature > waterRoomTemprature)
            {
                Temprature -= secondaryWaterCoolingRate * Time.deltaTime;
            }
        }

        string cookedNessDescriptor = "";
        if (occupiedVolume == 0)
        {
            objectName = "pot";
        }
        else if (containsCornflour || containsCelery || containsRutabaga)
        {
            float cookednessAddend = Temprature - waterSimmeringPoint;
            if (cookednessAddend > 0)
            {
                // Low liquid proportion exponentially expedites burning  
                contentsCookedness += (cookednessAddend * occupiedVolume * contentsCookingRate * Time.deltaTime) / liquidProportion;
            }
            if (contentsCookedness <= 0.25f)
            {
                cookedNessDescriptor = "uncooked";
            }
            else if (contentsCookedness <= 0.5f)
            {
                cookedNessDescriptor = "slightly cooked";
            }
            else if (contentsCookedness <= 0.75f)
            {
                cookedNessDescriptor = "partially cooked";
            }
            else if (contentsCookedness <= 1f)
            {
                cookedNessDescriptor = "cooked";
            }
            else if (contentsCookedness <= 1.25f)
            {
                cookedNessDescriptor = "overcooked";
            }
            else if (contentsCookedness <= 1.5)
            {
                cookedNessDescriptor = "burnt";
            }
            else if (contentsCookedness <= 1.75)
            {
                cookedNessDescriptor = "badly burnt";
            }
            else
            {
                cookedNessDescriptor = "incinerated";
            }

            if (containsCelery && containsRutabaga)
            {
                objectName = "pot of " + cookedNessDescriptor + " rutabaga and celery stew";
            }
            else if (containsRutabaga)
            {
                objectName = "pot of " + cookedNessDescriptor + " rutabaga stew";
            }
            else if (containsCelery)
            {
                objectName = "pot of " + cookedNessDescriptor + " celery stew";
            }
            else
            {
                objectName = "pot of " + cookedNessDescriptor;
                if (containsCornflour)
                {
                    objectName += " cornflour stew";
                }
            }
            consumableType = ConsumableObjectType.Edible;
        }
        else
        {
            currentlyConsumable = true;
            objectName = "pot of water";
            consumableType = ConsumableObjectType.Drinkable;
        }
        if (halfEaten)
        {
            objectName = "half-eaten " + objectName;
        }

        if (Temprature > waterBoilingPoint)
        {
            audioSource.volume = 0.5f;
        }
        else if (Temprature > waterSimmeringPoint)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            audioSource.volume = (Temprature - waterSimmeringPoint) / (waterBoilingPoint - waterSimmeringPoint) * 0.5f;
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }

        surfaceRenderer.material.color = Color.Lerp(surfaceColour, burntSurfaceColor, contentsCookedness - 1);
        surfaceTransform.transform.localPosition = Vector3.Lerp(surfacePositionEmpty, surfacePositionFull, occupiedVolume / capacity);
        surfaceTransform.transform.localScale = Vector3.Lerp(surfaceScaleEmpty, surfaceScaleFull, occupiedVolume / capacity);
    }

    public override InteractionTypes GetInteractionWith(InteractionObject targetObject)
    {
        if (targetObject is ObjectHolderHeatSource)
        {
            return InteractionTypes.Place;
        }
        else if (targetObject is ObjectHolderSink)
        {
            return InteractionTypes.Put;
        }
        else
        {
            return InteractionTypes.None;
        }
    }

    public override InteractionResult InteractWith(InteractionObject targetObject)
    {
        if (targetObject is ObjectHolderHeatSource)
        {
            ObjectHolderHeatSource targetObjectHolder = targetObject as ObjectHolderHeatSource;
            // It is necessary change transform parent as Interactant sets its  
            // HeldObjectState to Free upon release if still attached to char controller.
            transform.parent = null;
            transform.position = targetObjectHolder.heldObjectPosition;
            holdableObjectState = HoldableObjectStates.HeldByObjectHolder;
            mostRecentObjectHolder = targetObjectHolder;
            return new InteractionResult
            {
                Message = "You placed the pot on the " + targetObject.objectName + ".",
                Consequence = InteractionConsequence.ReleaseHeldObject
            };
        }

        else if (targetObject is ObjectHolderSink)
        {
            ObjectHolderSink targetObjectHolder = targetObject as ObjectHolderSink;
            // It is necessary change transform parent as Interactant sets its  
            // HeldObjectState to Free upon release if still attached to char controller.
            targetObjectHolder.pot = this;
            transform.parent = null;
            transform.position = targetObjectHolder.heldObjectPosition;
            transform.rotation = Quaternion.Euler(targetObjectHolder.heldObjectRotation);
            holdableObjectState = HoldableObjectStates.HeldByObjectHolder;
            mostRecentObjectHolder = targetObjectHolder;
            InteractionResult potPuttingResult = new InteractionResult();
            potPuttingResult.Message = "You put the pot in the " + targetObject.objectName + ".";
            potPuttingResult.Consequence = InteractionConsequence.ReleaseHeldObject;
            targetObjectHolder.PotFilledByFaucet = true;
            return potPuttingResult;
        }
        else
        {
            throw new ArgumentException("InteractWith called for InteractionObject that Pot cannot interact with."
            + "Check GetInteractionWith implementation.", "targetobject");
        }
    }

    public bool TryAddCornflour(CornflourHandful cornflourHandful)
    {
        if (containsCornflour || (occupiedVolume + cornflourHandful.ObjectVolume) > capacity)
        {
            return false;
        }
        else
        {
            surfaceColour = cornflourSurfaceColor;
            liquidProportion = (liquidProportion * (occupiedVolume / (occupiedVolume + cornflourHandful.ObjectVolume)))
                + (cornflourHandful.liquidProportion * (cornflourHandful.ObjectVolume / (occupiedVolume + cornflourHandful.ObjectVolume)));
            occupiedVolume += cornflourHandful.ObjectVolume;
            baseEnergyContentJ += cornflourHandful.EnergyContent;
            containsCornflour = true;
            return true;
        }
    }
    public bool TryAddCelery(ChoppedCelery choppedCelery)
    {
        if (containsCelery || (occupiedVolume + choppedCelery.ObjectVolume) > capacity)
        {
            return false;
        }
        else
        {
            celeryGameObject = Instantiate(celeryPrefabObject, surfaceTransform);
            celeryPrefabObject.transform.localPosition = new Vector3(0, 0, 0);
            liquidProportion = (liquidProportion * (occupiedVolume / (occupiedVolume + choppedCelery.ObjectVolume)))
                + (choppedCelery.liquidProportion * (choppedCelery.ObjectVolume / (occupiedVolume + choppedCelery.ObjectVolume)));
            baseEnergyContentJ += choppedCelery.EnergyContent;
            containsCelery = true;
            return true;
        }
    }
    public bool TryAddRutabaga(DicedRutabaga dicedRutabaga)
    {
        if (containsRutabaga || (occupiedVolume + dicedRutabaga.ObjectVolume) > capacity)
        {
            return false;
        }
        else
        {
            rutabagaGameObject = Instantiate(rutabagaPrefabObject, transform);
            rutabagaPrefabObject.transform.localPosition = rutabagaOffset;
            liquidProportion = (liquidProportion * (occupiedVolume / (occupiedVolume + dicedRutabaga.ObjectVolume)))
                + (dicedRutabaga.liquidProportion * (dicedRutabaga.ObjectVolume / (occupiedVolume + dicedRutabaga.ObjectVolume)));
            baseEnergyContentJ += dicedRutabaga.EnergyContent;
            containsRutabaga = true;
            return true;
        }
    }
    public bool TryAddWater(float addedVolume)
    {
        if ((occupiedVolume + addedVolume) < capacity)
        {
            occupiedVolume += addedVolume;
            return true;
        }
        else
        {
            return false;
        }
    }
}
