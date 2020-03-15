using UnityEngine;

public class Tap : InteractionObject {

	public float turnRate = 65f;
	public float turnCompletionDuration = 2f;
	private float tapState = 0f;
	private bool isTurning = false;
	private bool turnOn = true;
	
	public float state {
		get{return tapState;}
		set{tapState = value;}
	}
	void Update () {
		if (IsSelected){
			if (Input.GetButtonDown("Interact")){
				isTurning = true;
			}
			else if (Input.GetButtonUp("Interact")){
				isTurning = false;
			}
		}
		else if (isTurning) {
			isTurning = false;
		}
		if (isTurning) {
			if (turnOn) {
				tapState += (1/ turnCompletionDuration) * Time.deltaTime;
				transform.Rotate (0, turnRate * Time.deltaTime, 0);
				if (tapState >= 1) {
					isTurning = false;
					turnOn = false;
					tapState = 1;
				}
			}
			else {
				tapState -= (1 / turnCompletionDuration) * Time.deltaTime;
				transform.Rotate (0, -turnRate * Time.deltaTime, 0);
				if (tapState <= 0) {
					isTurning = false;
					turnOn = true;
					tapState = 0;
				}
			}
		}
	}
}
