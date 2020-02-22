using UnityEngine;
using System.Collections;

public class NPCInteraction : InteractionObject
{
    public int hitPoints = 11;
    public Metabolism NPCMetabolism;
    public Dialogue NPCDialogue;
    public AudioClip deathSound;
    // AudioClip Used by interaction logic based in other InteractionObject classes
    public AudioClip currentInteractionCLip;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        EventManager.StartListening("dialogue_1i", SetMetabolismAcceptingFoodFalse);
    }

    void SetMetabolismAcceptingFoodFalse()
    {
        NPCMetabolism.isAcceptingFood = false;
    }

    void Update()
    {
        if (IsSelected)
        {
            if (Input.GetButtonDown("Interact"))
            {
                NPCDialogue.StartConversing();
            }
        }
        if (NPCMetabolism.IsAlive)
        {
            if (hitPoints < 0)
            {
                NPCMetabolism.Kill();
                NPCDialogue.SetDead();
                audioSource.PlayOneShot(deathSound);
            }
        }
    }
}