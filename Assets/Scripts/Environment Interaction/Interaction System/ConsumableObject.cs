using System;
using UnityEngine;
using System.Collections;

public abstract class ConsumableObject : HoldableObject
{

    public bool currentlyConsumable = true;
    public bool isContainer = false;
    public float liquidProportion;
    public float baseEnergyContentJ; // The energy in jouls that would be contained in the object at a scale of 1
    public ConsumableObjectType consumableType;
    public bool isHalfable = false;
    public GameObject halfObject;
    public bool canBeBrokenUp;
    public GameObject brokenUpObject;
    public AudioClip consumptionClip;
    public AudioClip breakInHalfClip;
    public AudioClip breakUpClip;
    public float EnergyContent { get { return baseEnergyContentJ * MeanLocalScale; } }
    public float contentsCookedness = 0;
    protected float objectVolume;
    protected bool hasCaluclatedVolume = false;

    public virtual float ObjectVolume
    {
        get
        {
            if (!hasCaluclatedVolume)
            {
                MeshFilter meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
                objectVolume = MeshCalulate.Volume(meshFilter.sharedMesh) * Mathf.Pow(MeanLocalScale, 3);
                hasCaluclatedVolume = true;
            }
            return objectVolume;
        }
    }
    // Get  average scale in order to approximately scale nutritional values. 
    // Scale likely to be uniform anyway.
    protected float MeanLocalScale
    {
        get
        { return (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3; }
    }

    public virtual void InteractantHasConsumedHalfContents() { }

    public virtual void InteractantHasConsumedQuarterContents() { }

    public virtual void InteractantHasConsumedAllContents() { }

    void Start()
    {
        if (halfObject != null)
        {
            // Ensures that halves are scaled appropriately
            halfObject.transform.localScale = transform.localScale;
        }
    }

    public override InteractionResult InteractWith(InteractionObject targetObject)
    {
        InteractionResult interactionResult = new InteractionResult();
        if (targetObject is NPCInteraction)
        {
            NPCInteraction targetyNPCInteraction = targetObject as NPCInteraction;
            IngestionResult result = targetyNPCInteraction.NPCMetabolism.Ingest(this);
            this.name = this.objectName;
            switch (result)
            {
                case IngestionResult.IngestedAll:
                    interactionResult.Message = "You give the " + this.name + " to the " + targetObject.objectName + ".";
                    interactionResult.Consequence = InteractionConsequence.DestroyHeldObject;
                    targetyNPCInteraction.currentInteractionCLip = this.consumptionClip;
                    targetyNPCInteraction.audioSource.PlayOneShot(targetyNPCInteraction.currentInteractionCLip, 1f);
                    break;
                case IngestionResult.IngestedHalf:
                    interactionResult.Message = "You give half the " + this.objectName + " to the " + targetObject.objectName + ".";
                    interactionResult.Consequence = InteractionConsequence.HalfHeldObject;
                    targetyNPCInteraction.currentInteractionCLip = this.consumptionClip;
                    targetyNPCInteraction.audioSource.PlayOneShot(targetyNPCInteraction.currentInteractionCLip, 0.8f);
                    break;
                case IngestionResult.IngestedNone:
                    interactionResult.Message = "The " + targetObject.objectName + " will not take the " + this.objectName + ".";
                    interactionResult.Consequence = InteractionConsequence.RetainHeldObject;
                    break;
            }
            return interactionResult;
            // Returning null for benifit of decendant classes
        }
        else
        {
            return null;
        }
    }

    public void BreakInHalf()
    {
        if (halfObject != null)
        {
            GameObject firstHalf = (GameObject)Instantiate(halfObject, transform.position, transform.rotation);
            GameObject secondHalf = (GameObject)Instantiate(halfObject, transform.position, transform.rotation);
            secondHalf.transform.Rotate(5, 5, 185);

            AudioSource newObjectAudioSource = firstHalf.GetComponent<AudioSource>();
            AudioClip newObjectBreakInHalfClip = firstHalf.GetComponent<ConsumableObject>().breakInHalfClip;

            if (newObjectBreakInHalfClip != null)
            {
                newObjectAudioSource.PlayOneShot(newObjectBreakInHalfClip, 1f);
            }

        }
        else
        {
            throw new NullReferenceException("No GameObject is assigned to the public reference field 'half'. Halfing impossible.");
        }
    }
    public void BreakUp()
    {
        if (brokenUpObject != null)
        {
            GameObject dicedObject = (GameObject)Instantiate(brokenUpObject, transform.position, transform.rotation);

            AudioSource newObjectAudioSource = dicedObject.GetComponent<AudioSource>();
            AudioClip newObjectBreakUpClip = dicedObject.GetComponent<ConsumableObject>().breakUpClip;

            if (newObjectBreakUpClip != null)
            {
                newObjectAudioSource.PlayOneShot(newObjectBreakUpClip, 1f);
            }

        }
        else
        {
            throw new NullReferenceException("No GameObject is assigned to the public reference field 'brokenUpObject'. Breaking Up impossible.");
        }
    }

}

public enum ConsumableObjectType : byte { Edible, Drinkable, Usable }