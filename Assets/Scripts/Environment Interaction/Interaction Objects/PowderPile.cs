using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderPile : InteractionObject {

	public int numberOfUnits = 12;
	public Vector3 fullPosition;
	public Vector3 emptyPosition;
	public GameObject unitGameObject;
	public Interactant mainInteractant;
	private float unitsCount;

	void Start() {
		unitsCount = numberOfUnits;
	}

	void Update () {
		if (IsSelected && Input.GetButtonDown("Interact")) {
			GameObject newUnitObject = (GameObject)Instantiate(unitGameObject);
			mainInteractant.PickUpObject(newUnitObject.GetComponent<HoldableObject>());
			unitsCount--;
			transform.position = Vector3.Lerp (emptyPosition, fullPosition, (unitsCount + 1) / numberOfUnits);
			if (unitsCount == 0) {
				DestroyImmediate (this.gameObject);
			}
		}
	}
}
