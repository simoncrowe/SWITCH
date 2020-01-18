using System;
using UnityEngine;
using System.Collections;

public class KitchenKnife : HoldableObject
{

    public int damage = 4;
    public AudioClip[] stabSoundClips;
    private bool hasCutFirstNPC = false;

    private void Start()
    {
        EventManager.StartListening("dialogue_4", KnifeSpeak);
    }

    void KnifeSpeak()
    {
        Debug.Log("The knife speaks amind the silence of men!");
    }

    public override InteractionTypes GetInteractionWith(InteractionObject targetObject)
    {
        InteractionTypes avaliableInteractiontype = InteractionTypes.None;
        if (targetObject is Food)
        {
            Food targetFood = targetObject as Food;
            if (targetFood.canBeBrokenUp || targetFood.isHalfable)
            {
                avaliableInteractiontype = InteractionTypes.Cut;
            }
        }
        else if (targetObject is NPCInteraction)
        {
            avaliableInteractiontype = InteractionTypes.Cut;
        }
        return avaliableInteractiontype;
    }

    public override InteractionResult InteractWith(InteractionObject targetObject)
    {
        InteractionResult cuttingResult = new InteractionResult();
        cuttingResult.Consequence = InteractionConsequence.RetainHeldObject;
        if (targetObject is Food)
        {
            Food targetFoodItem = targetObject as Food;
            if (targetFoodItem.isHalfable)
            {
                targetFoodItem.BreakInHalf();
                GameObject.Destroy(targetFoodItem.transform.gameObject);
                cuttingResult.Message = "You cut the " + targetFoodItem.objectName + " in half.";
                return cuttingResult;
            }
            else if (targetFoodItem.canBeBrokenUp)
            {
                targetFoodItem.BreakUp();
                GameObject.Destroy(targetFoodItem.transform.gameObject);
                cuttingResult.Message = "You cut the " + targetFoodItem.objectName + " into pieces";
            }
            else
            {
                throw new ArgumentException("InteractWith called on InteractionObject that KitchenKnife cannot interact with."
                                            + "Check GetInteractionWith implementation.", "targetobject");
            }
        }
        else if (targetObject is NPCInteraction)
        {
            NPCInteraction targetNPC = targetObject as NPCInteraction;
            if (hasCutFirstNPC)
            {
                audioSource.PlayOneShot(stabSoundClips[UnityEngine.Random.Range(0, stabSoundClips.Length)], 1f);
            }
            else
            {
                audioSource.PlayOneShot(stabSoundClips[0], 1f);
                hasCutFirstNPC = true;
            }
            // Bad OOP, I know
            targetNPC.hitPoints -= damage;
        }
        else
        {
            throw new ArgumentException("InteractWith called on InteractionObject that KitchenKnife cannot interact with."
                                        + "Check GetInteractionWith implementation.", "targetobject");
        }
        return cuttingResult;
    }
}
