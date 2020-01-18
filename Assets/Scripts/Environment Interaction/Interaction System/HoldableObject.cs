using UnityEngine;
using System.Collections;

public abstract class HoldableObject : InteractionObject {
    public bool canBeDropped;
	public AudioClip collisionSound;
    public HoldableObjectStates holdableObjectState;
    public ObjectHolder mostRecentObjectHolder;

	protected AudioSource audioSource;

	void Awake() {
		audioSource = GetComponent<AudioSource> ();
	}

	void OnCollisionEnter(Collision collision) {
		// Only play collision audio for dropable objects
		// Assumes that all dropable objects have an AudioSource attached
		audioSource.PlayOneShot (collisionSound, collision.relativeVelocity.magnitude / 20f);
	}
	public virtual InteractionTypes GetInteractionWith (InteractionObject targetObject) {
		if (targetObject is NPCInteraction) {
			return InteractionTypes.Give;
		}
		return InteractionTypes.None;
	}

	public virtual void InteractantHasPickedUp() {}

	public virtual void InteractantHasDropped() {}

	public abstract InteractionResult InteractWith (InteractionObject targetObject);
}


public enum InteractionTypes: byte {None, Cut, Put, Place, Get, Give}
public enum HoldableObjectStates: byte {Free, HeldByInteractant, HeldByObjectHolder}

