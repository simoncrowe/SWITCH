using UnityEngine;
using System.Collections;

public class FrontWheelRotate : MonoBehaviour {

	public WheelCollider correspondingCollider;
	public WheelchairController wheelchairDriver;
	
	void FixedUpdate () {
		if (correspondingCollider.motorTorque < 0){
			transform.Rotate (wheelchairDriver.averageVelocity*9.831f,0,0);	
		}
		else {
			transform.Rotate (wheelchairDriver.averageVelocity*-9.831f,0,0);	
		}
	}
}