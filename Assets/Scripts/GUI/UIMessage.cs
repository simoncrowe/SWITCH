using System;


public class UIMessage: IComparable {
	public string Text{get; private set;}
	public bool ShouldDisplay{get; private set;}
	public float RemainingDuration {get; private set;}
	public UIMessageDisplayMode  DisplayMode {get; private set;}
	public int Priority {get; private set;}
	
	public void SetText(string newText) {Text = newText;}
	public void SetRemainingDuration(float newRemainingDuration) {RemainingDuration = newRemainingDuration;}
	public void SetDisplayMode(UIMessageDisplayMode newDisplayMode) {DisplayMode = newDisplayMode;}
	public void SetPriority (int newPriority) {Priority = newPriority;}
	public void SetShouldDiplay (bool shouldDisplay) {ShouldDisplay = shouldDisplay;}
	
	public UIMessage (string messageText, float messageRemainingDuration, 
	                  UIMessageDisplayMode displayMode, bool shouldDisplay, int priotity) {
		Text = messageText;
		RemainingDuration = messageRemainingDuration;
		DisplayMode = displayMode;
		Priority = priotity;
		ShouldDisplay = shouldDisplay;
	}

	public int CompareTo(object comparate) {
		UIMessage otherMessage = comparate as UIMessage;
		if (otherMessage != null) {
			return Priority.CompareTo(otherMessage.Priority);
		} else throw new ArgumentException("Comparate is not a UIMessage.");
	}
}

public enum UIMessageDisplayMode : byte {Continuous, Timed}