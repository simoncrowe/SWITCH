using UnityEngine;
using System.Collections;

public class WheelRotate : MonoBehaviour {
	
	public WheelCollider correspondingCollider;
	public WheelchairController wheelchairDriver;
	
	void FixedUpdate () {
		if (correspondingCollider.motorTorque < 0) {
			transform.Rotate (wheelchairDriver.averageVelocity*-3f,0,0);	
		}
		else {
			transform.Rotate (wheelchairDriver.averageVelocity*3f,0,0);				
		}
	}
}