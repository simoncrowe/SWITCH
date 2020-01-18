using UnityEngine;

public class Move : LerpTransform {
	
	public Vector3 endOffset;	
	
	void Start () {
		startVector = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
	}
	
	public override void ToEndOffset () {
		transform.position = Vector3.Lerp 
			(startVector, endOffset + startVector, (Time.time - startTime) / duration);
	}
	public override void FromEndOffset () {
			transform.position = Vector3.Lerp 
			(endOffset + startVector, startVector, (Time.time - startTime) / duration);
	}
}