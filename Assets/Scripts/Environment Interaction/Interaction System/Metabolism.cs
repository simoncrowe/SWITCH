using UnityEngine;
using System.Collections;

public class Metabolism : MonoBehaviour
{
    public bool IsAlive { get; private set; }
    public bool isAcceptingFood = true;
    public bool isPlayerCharacter;
    public bool isMobile;
    public InGameGUI inGameGUI;
    public float mass = 65; //kg
    public float bodyFatProportion = 0.2f;
    public float bodyProteinProportion = 0.17f;
    public float height = 1.7f;
    public float age;
    public Sex sex;
    public float metabolicSpeedScale = 1f;
    // Cubic meters
    public float stomachCapacity = 0.0015f;
    public float colonCapacity = 0.001f;
    public float colonFullness { get { return colonContents / colonCapacity; } }
    public float bladderCapacity = 0.0005f;
    public float bladderFullness { get { return bladderContents / bladderCapacity; } }
    public float EnergyHealth { get { return avaliableEnergy / startingAvailableEnergy; } }

    public float waterDigestionRate = 0.000000013f;     // Cubic metres per second
    public float solidDigestionRate = 0.00000000046f;   // Cubic metres per second
    private float avaliableEnergy; // Joules
    private float startingAvailableEnergy;
    public float DigestiveCapacityOccupied { get { return solidsDigesting + waterDigesting; } }
    public float energyDigesting = 2092000; // Joules
    public float solidsDigesting = 0.00025f;
    public float waterDigesting = 0.00045f;
    private float hungerThreshold = 0.1f;   // Minimum proportion of stomach containing solids 
                                            //before hunger messages display
    private float FreeStomachCapacity { get { return stomachCapacity - DigestiveCapacityOccupied; } }
    private float essentialEnergy; //Joules
    private float velocity;
    private float maxVitalBodyWater; // Maximum  proportion of body water maintained to prevent dehydration
    private float vitalBodyWater; // Body water preserving body from dehydrated
    private float environmentalBodyWaterLossRate = 0.000000009259259f;// Water lost through breath, perspiration ect. m^3/s
    private float urinaryBodyWaterLossRate = 0.00000001446759f;// Water lost though kidney processing. m^3/s
    private float colonContents;
    private float bladderContents;
    private Vector3 previousPosition;
    private float basalMetabolicRate;
    private UIMessage hungerMessage;
    private UIMessage thirstMessage;
    private AudioSource audioSource;

    public void Kill()
    {
        IsAlive = false;
    }
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        IsAlive = true;
        // calulates basal metabolic rate in jouls per second (Mifflin-St Jeor Equation)
        if (sex == Sex.female)
        {
            basalMetabolicRate =
                ((((9.99f * mass) + (625 * height) + (4.92f * age)) + 5) * 4184) / 86400;
            essentialEnergy = (mass * 1000 * bodyFatProportion * 36819.2f)
                - (mass * 1000 * (bodyFatProportion - 0.11f) * 36819.2f); // For measuring starvation
        }
        else
        {
            basalMetabolicRate =
                ((((9.99f * mass) + (625 * height) + (4.92f * age)) - 161) * 4184) / 86400;
            essentialEnergy = (mass * 1000 * bodyFatProportion * 36819.2f)
                - (mass * 1000 * (bodyFatProportion - 0.04f) * 37800); // For measuring starvation
        }

        // Approximates avaliable energy before starvation
        startingAvailableEnergy = 0.68f *       // adjusted based on 1987 hunger strike mortality data
                        ((500 * 16800f)                                     // Carbohydrates
                    + (mass * 1000 * bodyFatProportion * 36819.2f)          // Fat
                    + (mass * 1000 * bodyProteinProportion * 17154.4f));    // Protein 
        avaliableEnergy = startingAvailableEnergy;
        maxVitalBodyWater = ((mass * 0.65f /*percentage of body water*/)
                         / 1000) // Converting Kgs (litres) of water to cubic meters
            * 0.2f; // Approximate proportion of body water lost to cause dehydration.
        vitalBodyWater = maxVitalBodyWater * 0.8f;
    }
    void Start()
    {
        previousPosition = transform.position;
        hungerMessage = new UIMessage("You are hungry.", 0, UIMessageDisplayMode.Continuous, false, 0);
        thirstMessage = new UIMessage("You are thirsty.", 0, UIMessageDisplayMode.Continuous, false, 0);

        if (inGameGUI != null)
        {
            inGameGUI.AddUIMessage(hungerMessage);
            inGameGUI.AddUIMessage(thirstMessage);
        }
    }

    void FixedUpdate()
    {
        if (IsAlive)
        {
            //print ("Metabolic days past: " + (Time.time*metabolicSpeedScale)/86400 + "  Avaliable energy: " + avaliableEnergy);
            if (solidsDigesting > 0)
            {
                float solidEnergyTransfer = energyDigesting
                    * ((metabolicSpeedScale * solidDigestionRate * Time.fixedDeltaTime) / solidsDigesting);
                energyDigesting -= solidEnergyTransfer;
                avaliableEnergy += solidEnergyTransfer;
                solidsDigesting -= solidDigestionRate * Time.fixedDeltaTime * metabolicSpeedScale;
                colonContents += solidDigestionRate * Time.fixedDeltaTime * metabolicSpeedScale;

            }
            if (waterDigesting > 0)
            {
                waterDigesting -= waterDigestionRate * Time.fixedDeltaTime * metabolicSpeedScale;
                if (vitalBodyWater < maxVitalBodyWater) // Does the body need water?
                {
                    // Put the water into bodily circulation.
                    vitalBodyWater += waterDigestionRate * Time.fixedDeltaTime * metabolicSpeedScale;
                }
                else
                {
                    // Put the water into the bladder.
                    bladderContents += waterDigestionRate * Time.fixedDeltaTime * metabolicSpeedScale;
                }

            }
            // Apply environmental water loss
            vitalBodyWater -= environmentalBodyWaterLossRate * Time.fixedDeltaTime * metabolicSpeedScale;
            // Apply urinary water loss
            vitalBodyWater -= urinaryBodyWaterLossRate * Time.fixedDeltaTime * metabolicSpeedScale;
            bladderContents += urinaryBodyWaterLossRate * Time.fixedDeltaTime * metabolicSpeedScale;

            // subtract energy for metabolic functions
            avaliableEnergy -= basalMetabolicRate * Time.fixedDeltaTime * metabolicSpeedScale;
            // subtract energy for movement (using meters per step rather than per second)
            if (isMobile)
            {
                Vector3 positionChange = transform.position - previousPosition;
                float velocity = positionChange.magnitude;
                avaliableEnergy -= ((mass * (Mathf.Pow(velocity, 2))) / 2) * metabolicSpeedScale;
                previousPosition = transform.position;
            }   // If player character, Display UI messages for metabolic state
            if (isPlayerCharacter)
            {
                if (solidsDigesting < (stomachCapacity * hungerThreshold))
                {
                    hungerMessage.SetShouldDiplay(true);
                    hungerMessage.SetText("You feel peckish.");
                    if (solidsDigesting < (stomachCapacity * (hungerThreshold / 4)))
                    {
                        hungerMessage.SetText("You are hungry. Consider eating.");
                    }
                    if ((solidsDigesting < (stomachCapacity * (hungerThreshold / 16))) &&
                        (EnergyHealth < 0.85))
                    {
                        hungerMessage.SetText("You feel desperately ravenous.");
                    }
                    if ((solidsDigesting < (stomachCapacity * (hungerThreshold / 16))) &&
                        (avaliableEnergy < (essentialEnergy) /*Energy apart from non-essential fats*/))
                    {
                        hungerMessage.SetText("You are starving. Eat soon.");
                    }
                    if ((solidsDigesting < (stomachCapacity * (hungerThreshold / 32))) &&
                        (avaliableEnergy < 41840000 /*10000kcal*/))
                    {
                        hungerMessage.SetText("You feel famished. You will die if you do not eat.");
                    }
                }
                else
                {
                    hungerMessage.SetShouldDiplay(false);
                }
                if (vitalBodyWater < (maxVitalBodyWater / 3))
                {
                    thirstMessage.SetShouldDiplay(true);
                    thirstMessage.SetText("You are thirsty.");
                    if (vitalBodyWater < (maxVitalBodyWater / 9))
                    {
                        thirstMessage.SetText("You feel parched and could really do with some water.");
                    }
                    if (vitalBodyWater < (maxVitalBodyWater / 27))
                    {
                        thirstMessage.SetText("You are dehydrated. If you don't don't take on some water soon, you will die.");
                    }
                    if (vitalBodyWater < 0) { IsAlive = false; print("You have died of dehydration!"); }
                }
                else
                {
                    thirstMessage.SetShouldDiplay(false);
                }
                if (avaliableEnergy < 0) { IsAlive = false; print("You have died of malnutrition!"); }

            }
        }
    }

    public IngestionResult Ingest(ConsumableObject ingestedObject)
    {
        if (isAcceptingFood)
        {
            if (ingestedObject.isContainer)
            {
                if (ingestedObject.ObjectVolume < FreeStomachCapacity)
                {
                    energyDigesting += ingestedObject.EnergyContent;
                    waterDigesting += (ingestedObject.ObjectVolume * ingestedObject.liquidProportion);
                    solidsDigesting += (ingestedObject.ObjectVolume * (1 - ingestedObject.liquidProportion));
                    return IngestionResult.IngestedAllContents;
                }
                else if (ingestedObject.ObjectVolume / 2 < FreeStomachCapacity)
                {
                    energyDigesting += ingestedObject.EnergyContent / 2;
                    waterDigesting += (ingestedObject.ObjectVolume * ingestedObject.liquidProportion) / 2;
                    solidsDigesting += (ingestedObject.ObjectVolume * (1 - ingestedObject.liquidProportion)) / 2;
                    return IngestionResult.IngestedHalfContents;
                }
                else if (ingestedObject.ObjectVolume / 4 < FreeStomachCapacity)
                {
                    energyDigesting += ingestedObject.EnergyContent / 4;
                    waterDigesting += (ingestedObject.ObjectVolume * ingestedObject.liquidProportion) / 4;
                    solidsDigesting += (ingestedObject.ObjectVolume * (1 - ingestedObject.liquidProportion)) / 4;
                    return IngestionResult.IngestedQuarterContents;
                }
            }
            else if (ingestedObject.ObjectVolume < FreeStomachCapacity)
            {
                energyDigesting += ingestedObject.EnergyContent;
                waterDigesting += ingestedObject.ObjectVolume * ingestedObject.liquidProportion;
                solidsDigesting += ingestedObject.ObjectVolume * (1 - ingestedObject.liquidProportion);
                return IngestionResult.IngestedAll;
            }
            else if (ingestedObject.isHalfable)
            {
                ConsumableObject halfIngestedObject = (ConsumableObject)
                    ingestedObject.halfObject.GetComponent(typeof(ConsumableObject));
                if (halfIngestedObject.ObjectVolume < (stomachCapacity - DigestiveCapacityOccupied))
                {
                    energyDigesting += halfIngestedObject.EnergyContent;
                    waterDigesting += halfIngestedObject.ObjectVolume * (ingestedObject.liquidProportion);
                    solidsDigesting += halfIngestedObject.ObjectVolume * (1 - ingestedObject.liquidProportion);
                    return IngestionResult.IngestedHalf;
                }
            }
        }
        return IngestionResult.IngestedNone;
    }

    public SimpleConsumable IngestSimple(SimpleConsumable consumable, AudioClip ingestionClip)
    {
        float consumedVolume;
        float consumedEnergy;

        if (consumable.Volume > 0 && FreeStomachCapacity > 0)
        {
            audioSource.PlayOneShot(ingestionClip);
        }

        if (consumable.Volume > FreeStomachCapacity)
        {
            consumedVolume = FreeStomachCapacity;
            consumedEnergy = consumable.Energy * (consumedVolume / consumable.Volume);
        }
        else
        {
            consumedVolume = consumable.Volume;
            consumedEnergy = consumable.Energy;
        }

        consumable.Energy -= consumedEnergy;
        energyDigesting += consumedEnergy;

        consumable.Volume -= consumedVolume;
        solidsDigesting += consumedVolume / 2;
        waterDigesting += consumedVolume / 2;

        return consumable;
    }
}

public enum IngestionResult { IngestedAll, IngestedHalf, IngestedNone, IngestedAllContents, IngestedHalfContents, IngestedQuarterContents }

public enum Sex { female, male }
