using System;
using UnityEngine;
using System.Collections;

public class DicedRutabaga : Food
{

    public override InteractionTypes GetInteractionWith(InteractionObject targetObject)
    {
        if (targetObject is Pot)
        {
            return InteractionTypes.Put;
        }
        return base.GetInteractionWith(targetObject);
    }

    public override InteractionResult InteractWith(InteractionObject targetObject)
    {
        InteractionResult baseResult = base.InteractWith(targetObject);
        if (baseResult != null)
        {
            return baseResult;
        }
        else if (targetObject is Pot)
        {
            Pot targetPot = targetObject as Pot;
            InteractionResult result = new InteractionResult();
            if (targetPot.TryAddRutabaga(this))
            {
                result.Consequence = InteractionConsequence.DestroyHeldObject;
                result.Message = "You put the rutabaga in the pot.";
            }
            else
            {
                result.Consequence = InteractionConsequence.RetainHeldObject;
                result.Message = "The diced rutabaga will not fit in the pot.";
            }
            return result;
        }
        else
        {
            throw new ArgumentException("InteractWith called on InteractionObject that DicedRutabaga cannot interact with."
                + "Check GetInteractionWith implementation.", "targetobject");
        }
    }

}
