using UnityEngine;
using System.Collections;

public class ForkRotate : MonoBehaviour {
	
	public WheelCollider correspondingCollider;
	
	void Update () {
		transform.localEulerAngles = new Vector3 (270,180,correspondingCollider.steerAngle);
	}
}
