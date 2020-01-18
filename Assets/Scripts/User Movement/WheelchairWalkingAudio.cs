using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelchairWalkingAudio : MonoBehaviour {

	public float minStepPauseDuration = 0.5f;
	public float maxStepPauseDuration = 0.6f;
	public AudioClip[] footstepAudio;
	public WheelchairController wheelchairControler;

	private float nextStepTime = 0;
	private AudioSource audioSource;
	private float averageVelocity;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	void Update () {
		averageVelocity = Mathf.Abs((wheelchairControler.getRigitBody.velocity.x 
									+ wheelchairControler.getRigitBody.velocity.y 
									+ wheelchairControler.getRigitBody.velocity.z)/3f);
		
		if (averageVelocity > 0.1f && wheelchairControler.isPushing) {
			if (Time.time > nextStepTime) {
				audioSource.PlayOneShot (footstepAudio[Random.Range (0, footstepAudio.Length)]);
				nextStepTime = Time.time + Random.Range (minStepPauseDuration, maxStepPauseDuration);
			}
		}
	}
}
