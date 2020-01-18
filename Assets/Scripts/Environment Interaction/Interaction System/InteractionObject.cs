using UnityEngine;
using System.Collections;

public abstract class InteractionObject : MonoBehaviour {

	public static bool intaractionMenuActive = false; 	
	public bool IsSelected {get; set;}
	public string objectName;
}