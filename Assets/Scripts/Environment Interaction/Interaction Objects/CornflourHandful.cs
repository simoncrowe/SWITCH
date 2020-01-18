using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornflourHandful : Food {

	public override InteractionTypes GetInteractionWith (InteractionObject targetObject) {
		if (targetObject is Pot) {
			return InteractionTypes.Put;
		}
		return base.GetInteractionWith (targetObject);
	}

	public override InteractionResult InteractWith (InteractionObject targetObject) {
		InteractionResult baseResult = base.InteractWith (targetObject);
		if (baseResult != null) {
			return baseResult;
		} else if (targetObject is Pot) {
			Pot targetPot = targetObject as Pot;
			InteractionResult result = new InteractionResult ();
			if (targetPot.TryAddCornflour (this)) {
				result.Consequence = InteractionConsequence.DestroyHeldObject;
				result.Message = "You put the cornflour in the pot.";
			} else {
				result.Consequence = InteractionConsequence.RetainHeldObject;
				result.Message = "There is no room in the pot for the cornflour.";
			}
			return result;
		} else {
			throw new ArgumentException("InteractWith called on InteractionObject that CournflourHandful cannot interact with."
				+ "Check GetInteractionWith implementation.", "targetobject");
		}
		return null;
	}
}
