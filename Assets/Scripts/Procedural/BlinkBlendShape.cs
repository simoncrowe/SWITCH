using UnityEngine;
using System.Collections;

public class BlinkBlendShape : BlendShape {
	public Metabolism manMetabolism;

	float lastBlinkEnd = 0;
	float blinkStart;
	float lerpEnd;
	float currentWait;
	bool blinking;
	bool hasStarted;
	bool hasPaused;
	public float wait = 6;
	public float waitRandomMin = -0.75f;
	public float waitRanomMax = 0.75f;
	public float startDuration = 0.08f;
	public float pauseDuration = 0.05f;
	public float endDuration = 0.075f;
	
	protected override void Initialize() {
		currentWait = wait + Random.Range(waitRandomMin, waitRanomMax);
	}
	protected override void ProcessWeights () {	
		if (manMetabolism.IsAlive && blinking) {
			if (!hasStarted) {
				if (Time.time < lerpEnd) {
					weights[0] = 1 - 
						((lerpEnd - Time.time) / startDuration);
				} else {
					hasStarted = true;
					lerpEnd = Time.time + pauseDuration;
				}
			} else if (!hasPaused) {
				if (Time.time > lerpEnd) {
					hasPaused = true;
					lerpEnd = Time.time + endDuration;
				}
			} else {
				if (Time.time < lerpEnd) {
					weights[0] = (lerpEnd - Time.time) / endDuration;
				} else {
					blinking = false;	
					lastBlinkEnd = Time.time;
					currentWait = wait 
						+ Random.Range(waitRandomMin, waitRanomMax);
					hasStarted = false;
					hasPaused = false;
				}
			}
		} else {
			if (Time.time >= lastBlinkEnd + currentWait) {
				blinking = true;
				lerpEnd = Time.time + startDuration;
			}
		}
	}
}