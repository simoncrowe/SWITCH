using UnityEngine;
using System.Collections;
using System;

public class Interactant : MonoBehaviour
{

    public InGameGUIMotile inGameGUI;
    public Metabolism metabolism;
    public float selectionCastDistance = 1.7f;
    public Vector3 heldGameObjectOffset;


    private InteractionObject selectedObject;
    private HoldableObject heldInteractionObject;
    private MeshRenderer heldMeshRenderer;
    private GameObject heldGameObject;
    private Rigidbody heldGameObjectRigidBody;
    private Ray selectionRay;
    private RaycastHit targetHit;
    private Vector2 middle;
    private string[] ingestionFailurePhrases;
    private UIMessage consumeObjectMessage;
    private UIMessage dropObjectMessage;
    private UIMessage ingestionResultMessage;
    private UIMessage interactionResultMessage;
    private UIMessage interactWithSelectedMessage;
    private AudioSource audioSource;
    private AudioClip currentConsumptionClip;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        middle = new Vector2(0.5f, 0.5f);
        ingestionFailurePhrases = new string[] { "fail to ", "cannot", "can't bring yourself to" };
        consumeObjectMessage = new UIMessage("Press E to eat ####",
                                       0, UIMessageDisplayMode.Continuous, false, 2);
        dropObjectMessage = new UIMessage("Press Q to drop ####",
                                      0, UIMessageDisplayMode.Continuous, false, 1);
        ingestionResultMessage = new UIMessage("You have eaten the ####",
                                    0, UIMessageDisplayMode.Timed, false, 3);
        interactionResultMessage = new UIMessage("You put X in Y",
                                    0, UIMessageDisplayMode.Timed, false, 4);
        interactWithSelectedMessage = new UIMessage("Put the lotion in the basket",
                                       0, UIMessageDisplayMode.Continuous, false, 0);
        inGameGUI.AddUIMessage(consumeObjectMessage);
        inGameGUI.AddUIMessage(dropObjectMessage);
        inGameGUI.AddUIMessage(ingestionResultMessage);
        inGameGUI.AddUIMessage(interactionResultMessage);
        inGameGUI.AddUIMessage(interactWithSelectedMessage);
    }

    void Update()
    {
        selectionRay = GetComponent<Camera>().ViewportPointToRay(middle);
        if (Physics.Raycast(selectionRay, out targetHit, selectionCastDistance))
        {
            DisplayAppropriateCursor();
            // If targetHit is distinct from the current selectedObject, the current selectedObject is 
            // no longer selected
            if ((selectedObject != null) &&
                (selectedObject.transform != targetHit.transform))
            {
                selectedObject.IsSelected = false;
            }
            selectedObject = targetHit.transform.gameObject.GetComponent
                ("InteractionObject") as InteractionObject;
            // If there is a selected object, per-existing or new, it is now selected
            if (selectedObject != null)
            {
                selectedObject.IsSelected = true;
            }
            // If no interactionObject is hit, the current one is deselected
        }
        else if (selectedObject != null)
        {
            inGameGUI.CurrentCursor = InGameGUIMotile.CursorType.Normal;
            selectedObject.IsSelected = false;

        }
        // The Interactant isn't holding anything
        if (heldGameObject == null)
        {
            // Is current selectedObject  holdable?
            if ((selectedObject != null) && (selectedObject is HoldableObject))
            {
                // Interactant activates selected holdable object
                if (Input.GetButtonDown("Interact"))
                {
                    PickUpObject(selectedObject as HoldableObject);
                    selectedObject = null;
                }
            }
            // The Interactant is holding something
        }
        else
        {
            // Logic relating to interaction with other objects in scene
            ProcessHeldAndSelectedObjectInteraction();
            // Interactant drops object
            if (heldInteractionObject.canBeDropped && Input.GetButtonDown("DropObject"))
            {
                ReleaseHeldObject();
                // Interactant continues to hold object
            }
            else
            {
                // The object is consumable
                ProcessHeldObjectConsumption();

            }
        }
    }

    private void DisplayAppropriateCursor()
    {
        if ((targetHit.transform.tag == "Usable") || (targetHit.transform.tag == "Holdable"))
        {
            inGameGUI.CurrentCursor = InGameGUIMotile.CursorType.Use;
        }
        else if (targetHit.transform.tag == "Conversable")
        {
            inGameGUI.CurrentCursor = InGameGUIMotile.CursorType.Converse;
        }
        else
        {
            inGameGUI.CurrentCursor = InGameGUIMotile.CursorType.Normal;
        }
    }

    private void ProcessHeldObjectConsumption()
    {
        if ((heldInteractionObject.GetType() == typeof(ConsumableObject))
            || (heldInteractionObject.GetType().IsSubclassOf(typeof(ConsumableObject))))
        {
            ConsumableObject heldConsumable = heldInteractionObject as ConsumableObject;
            if (heldConsumable.currentlyConsumable)
            {
                string ingestionVerb = string.Empty;
                // Set and display UI message for object consumption 
                switch (heldConsumable.consumableType)
                {
                    case ConsumableObjectType.Drinkable:
                        ingestionVerb = "drink";
                        break;
                    case ConsumableObjectType.Edible:
                        ingestionVerb = "eat";
                        break;
                    case ConsumableObjectType.Usable:
                        ingestionVerb = "use";
                        break;
                }
                consumeObjectMessage.SetText("Press E to " + ingestionVerb + " the " + heldConsumable.objectName + ".");
                consumeObjectMessage.SetShouldDiplay(true);
                // Interactant attempts to consume object
                if (Input.GetButtonDown("ConsumeObject"))
                {
                    IngestionResult result = metabolism.Ingest(heldConsumable);
                    switch (result)
                    {
                        case IngestionResult.IngestedAll:
                            ingestionResultMessage.SetText("You " + ingestionVerb + " the " + heldConsumable.objectName);
                            currentConsumptionClip = heldConsumable.consumptionClip;
                            audioSource.PlayOneShot(currentConsumptionClip, 1f);
                            DestroyHeldObject();
                            break;
                        case IngestionResult.IngestedHalf:
                            ingestionResultMessage.SetText("You " + ingestionVerb + " the " + heldConsumable.halfObject);
                            currentConsumptionClip = heldConsumable.consumptionClip;
                            audioSource.PlayOneShot(currentConsumptionClip, 0.8f);
                            HalfHeldObject(heldConsumable);
                            break;
                        case IngestionResult.IngestedNone:
                            string ingestionFailPhrase = ingestionFailurePhrases[UnityEngine.Random.Range(0, ingestionFailurePhrases.Length)];
                            ingestionResultMessage.SetText("You " + ingestionFailPhrase + " " + ingestionVerb + " the " + heldConsumable.objectName + ".");
                            break;
                        case IngestionResult.IngestedAllContents:
                            ingestionResultMessage.SetText("You " + ingestionVerb + " the " + heldConsumable.objectName);
                            heldConsumable.InteractantHasConsumedAllContents();
                            break;
                        case IngestionResult.IngestedHalfContents:
                            ingestionResultMessage.SetText("You " + ingestionVerb + " half of the " + heldConsumable.objectName);
                            heldConsumable.InteractantHasConsumedHalfContents();
                            break;
                    }
                    ingestionResultMessage.SetRemainingDuration(3f);
                    ingestionResultMessage.SetShouldDiplay(true);
                }
            }
        }
    }

    private void ProcessHeldAndSelectedObjectInteraction()
    {
        InteractionTypes avaliableInteraction = heldInteractionObject.GetInteractionWith(selectedObject);
        string interactionSentence = string.Empty;
        if (avaliableInteraction != InteractionTypes.None)
        {
            switch (avaliableInteraction)
            {
                case InteractionTypes.Cut:
                    interactionSentence = "Press F to cut the " + selectedObject.objectName
                        + " with the " + heldInteractionObject.objectName + ".";
                    break;
                case InteractionTypes.Get:
                    interactionSentence = "Press F to get the " + selectedObject.objectName
                        + " from the " + heldInteractionObject.objectName + ".";
                    break;
                case InteractionTypes.Give:
                    interactionSentence = "Press F to give the " + heldInteractionObject.objectName
                        + " to the " + selectedObject.objectName + ".";
                    break;
                case InteractionTypes.Put:
                    interactionSentence = "Press F to put the " + heldInteractionObject.objectName
                        + " in the " + selectedObject.objectName + ".";
                    break;
                case InteractionTypes.Place:
                    interactionSentence = "Press F to place the " + heldInteractionObject.objectName
                        + " on the " + selectedObject.objectName + ".";
                    break;
            }
            interactWithSelectedMessage.SetText(interactionSentence);
            interactWithSelectedMessage.SetShouldDiplay(true);
            InteractionResult interactionResult = new InteractionResult();
            // Is the user pressing the interact button?
            if (Input.GetButtonDown("InteractObject"))
            {
                interactionResult = heldInteractionObject.InteractWith(selectedObject);
                interactionResultMessage.SetText(interactionResult.Message);
                // Respond to interaction results
                switch (interactionResult.Consequence)
                {
                    case InteractionConsequence.ReleaseHeldObject:
                        ReleaseHeldObject();
                        break;
                    case InteractionConsequence.DestroyHeldObject:
                        DestroyHeldObject();
                        break;
                    case InteractionConsequence.HalfHeldObject:
                        HalfHeldObject(heldInteractionObject as ConsumableObject);
                        break;
                }
                interactWithSelectedMessage.SetShouldDiplay(false);
                interactionResultMessage.SetRemainingDuration(3f);
                interactionResultMessage.SetShouldDiplay(true);
            }
        }
        else
        {
            interactWithSelectedMessage.SetShouldDiplay(false);

        }
    }

    public void PickUpObject(HoldableObject objectToPickUp)
    {

        heldInteractionObject = objectToPickUp;
        heldInteractionObject.holdableObjectState = HoldableObjectStates.HeldByInteractant;
        heldInteractionObject.InteractantHasPickedUp();

        heldGameObject = objectToPickUp.gameObject;
        heldGameObject.transform.SetParent(this.gameObject.transform.parent.transform); // Parent the held object to the character controller

        heldMeshRenderer = (MeshRenderer)heldGameObject.GetComponent(typeof(MeshRenderer));
        heldMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        heldGameObjectRigidBody = (Rigidbody)
            heldGameObject.gameObject.GetComponent(typeof(Rigidbody));
        heldGameObjectRigidBody.isKinematic = true;
        heldGameObject.transform.localPosition = heldGameObjectOffset;
        heldGameObject.transform.rotation = Quaternion.identity;
        heldGameObject.layer = 2;
        if (heldInteractionObject.canBeDropped)
        {
            dropObjectMessage.SetText("Press Q to drop " + heldInteractionObject.objectName + ".");
            dropObjectMessage.SetShouldDiplay(true);
        }
    }

    private void ReleaseHeldObject()
    {
        // ensure that the interactan's parent is still object's parent before unparenting.

        if (System.Object.ReferenceEquals(heldGameObject.transform.parent,
            this.gameObject.transform.parent.transform))
        {
            heldGameObject.transform.SetParent(null);
            heldInteractionObject.holdableObjectState = HoldableObjectStates.Free;
            heldInteractionObject.InteractantHasDropped();
        }
        // Check whether held object can be dropped or should remain kinematic
        if (heldInteractionObject.canBeDropped)
        {
            heldGameObjectRigidBody.isKinematic = false;
        }
        heldMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        heldGameObject.layer = 0;
        heldGameObject = null;
        heldGameObjectRigidBody = null;
        heldMeshRenderer = null;
        dropObjectMessage.SetShouldDiplay(false);
        consumeObjectMessage.SetShouldDiplay(false);
    }

    private void DestroyHeldObject()
    {
        GameObject.DestroyImmediate(heldGameObject);
        heldInteractionObject = null;
        heldMeshRenderer = null;
        heldGameObjectRigidBody = null;
        heldGameObject = null;
        dropObjectMessage.SetShouldDiplay(false);
        consumeObjectMessage.SetShouldDiplay(false);
    }

    private void HalfHeldObject(ConsumableObject heldConsumable)
    {
        GameObject halfObject = (GameObject)Instantiate(heldConsumable.halfObject,
                                                        heldGameObject.transform.position,
            heldGameObject.transform.rotation);
        //Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360))));
        GameObject.DestroyImmediate(heldGameObject);

        heldInteractionObject = null;
        heldMeshRenderer = null;
        heldGameObjectRigidBody = null;

        heldGameObject = halfObject;
        heldGameObject.transform.SetParent(this.gameObject.transform.parent.transform);

        heldInteractionObject = heldGameObject.GetComponent<HoldableObject>();
        heldInteractionObject.holdableObjectState = HoldableObjectStates.HeldByInteractant;

        heldMeshRenderer = heldGameObject.GetComponent<MeshRenderer>();
        heldMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        heldGameObjectRigidBody = heldGameObject.GetComponent<Rigidbody>();
        heldGameObjectRigidBody.isKinematic = true;
        heldGameObject.transform.localPosition = heldGameObjectOffset;
        heldGameObject.transform.rotation = Quaternion.identity;
        heldGameObject.layer = 2;
        if (heldInteractionObject.canBeDropped)
        {
            dropObjectMessage.SetText("Press Q to drop " + heldInteractionObject.objectName + ".");
            dropObjectMessage.SetShouldDiplay(true);
        }
    }
}
