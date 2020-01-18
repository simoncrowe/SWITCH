using UnityEngine;

//Instead of defining Awake(), override LateAwake()

public abstract class LerpTransform : InteractionObject {
	
	public float duration = 1f;
	public AudioClip startTransformStartSound;
	public AudioClip startTransformEndSound;
	public AudioClip endTransformStartSound;
	public AudioClip endTransformEndSound;
	protected bool atEndTransform = false;
	protected bool transforming = false;
	protected bool hasSetTime = false;
	private bool hasPlayedStartTransformStartSound = false;
	private bool hasPlayedEndTransformStartSound = false;
	protected float endTime;
	protected float startTime;
	protected Vector3 startVector;
	protected AudioSource audioSource;

	void Awake() {
		audioSource = GetComponent<AudioSource> ();
	}

	void Update () {
		if (transforming) {
			if (hasSetTime) {
				if (atEndTransform) {
					if (Time.time < endTime){
						if (!hasPlayedEndTransformStartSound) {
							if (endTransformStartSound != null) {
								audioSource.PlayOneShot (endTransformStartSound);
							}
							hasPlayedEndTransformStartSound = true;
						}
						FromEndOffset();
					}
					else {
						if (endTransformEndSound != null) {
							audioSource.PlayOneShot (endTransformEndSound);
						}
						hasPlayedEndTransformStartSound = false;
						transforming = false;
						hasSetTime = false;
						atEndTransform = false;	
					}
				} 
				else {
					if (Time.time < endTime) {
						if (!hasPlayedStartTransformStartSound) {
							if (startTransformStartSound != null) {
								audioSource.PlayOneShot (startTransformStartSound);
							}
							hasPlayedStartTransformStartSound = true;
						}
						ToEndOffset();
					}
					else {
						if (startTransformEndSound != null) {
							audioSource.PlayOneShot (startTransformEndSound);
						}
						hasPlayedStartTransformStartSound = false;
						transforming = false;
						hasSetTime = false;
						atEndTransform = true;
					}
				}
			} 
			else {
				endTime = Time.time + duration;
				startTime = Time.time;
				hasSetTime = true;
			}
		} 
		else if (IsSelected) {
			if (Input.GetButtonDown("Interact")) {
				transforming = true;
			}
		}
	}
	public abstract void ToEndOffset();
	public abstract void FromEndOffset();
}