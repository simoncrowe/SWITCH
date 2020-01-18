using UnityEngine;


public class Scale : LerpTransform {
	
	public Vector3 endScale;
	
	void Start () {
		startVector = new Vector3 (transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
	
	public override void ToEndOffset () {
		transform.localScale = Vector3.Lerp 
			(startVector, endScale, (Time.time - startTime) / duration);
	}
	public override void FromEndOffset () {
			transform.localScale = Vector3.Lerp 
			(endScale, startVector, (Time.time - startTime) / duration);
	}
}