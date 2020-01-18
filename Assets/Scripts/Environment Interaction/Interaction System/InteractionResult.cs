using UnityEngine;
using System.Collections;

public class InteractionResult {
    public string Message;
    public InteractionConsequence Consequence; 
}
public enum InteractionConsequence: byte {RetainHeldObject, ReleaseHeldObject, HalfHeldObject, DestroyHeldObject}
