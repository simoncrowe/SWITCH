using UnityEngine;


public class Rotate : LerpTransform {
	
	public Vector3 endOffset;	
	private float lerpTime;
	
	void Start () {
		startVector = new Vector3 (transform.localEulerAngles.x,
			transform.localEulerAngles.y, transform.localEulerAngles.z);
	}
	public override void ToEndOffset () {
		lerpTime = (Time.time - startTime) / duration;
		transform.localEulerAngles = new Vector3
			(Mathf.LerpAngle (startVector.x, endOffset.x, lerpTime),
			Mathf.LerpAngle (startVector.y, endOffset.y, lerpTime),
			Mathf.LerpAngle (startVector.z, endOffset.z, lerpTime));
	}
	public override void FromEndOffset () {
		lerpTime = (Time.time - startTime) / duration;
		transform.localEulerAngles = new Vector3 
			(Mathf.LerpAngle (endOffset.x, startVector.x, lerpTime),
			Mathf.LerpAngle (endOffset.y, startVector.y, lerpTime),
			Mathf.LerpAngle (endOffset.z, startVector.z, lerpTime));
	}
}