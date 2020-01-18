using System;
using UnityEngine;
using System.Collections;

public class ChoppedCelery : Food {

	public override float ObjectVolume {
		get {
			return 0.00005f;
		}
	}

	public override InteractionTypes GetInteractionWith (InteractionObject targetObject) {
		if (targetObject is Pot) {
			return InteractionTypes.Put;
		}
		return base.GetInteractionWith(targetObject);
	}
	public override InteractionResult InteractWith (InteractionObject targetObject) {
		InteractionResult baseResult = base.InteractWith (targetObject);
		if (baseResult != null) {
			return baseResult;
		} else if (targetObject is Pot) {
			Pot targetPot = targetObject as Pot;
			InteractionResult result = new InteractionResult ();
			if (targetPot.TryAddCelery (this)) {
				result.Consequence = InteractionConsequence.DestroyHeldObject;
				result.Message = "You put the chopped celery in the pot.";
			} else {
				result.Consequence = InteractionConsequence.RetainHeldObject;
				result.Message = "There is no room in the pot for the celery.";
			}
			return result;
		} else {
			throw new ArgumentException("InteractWith called on InteractionObject that ChoppedCelery cannot interact with."
				+ "Check GetInteractionWith implementation.", "targetobject");
		}
		return null;
	}

}
