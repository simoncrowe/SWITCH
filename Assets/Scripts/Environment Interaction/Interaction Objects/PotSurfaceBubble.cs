using UnityEngine;
using System.Collections;

public class PotSurfaceBubble : RippleTexture {
	public Pot pot;
	public Faucet faucet;
	public bool PotIsInSkink{ get; set;}
    public int minBubbleRateBoiling;
    public int maxBubbleRateBoiling;
    public int bubbleFreeBorder = 4;
    public float bubbleForceScaleWeight = 0.1f;
    public float bubbleIntervalScaleWeight = 0.1f;
	public float minSplashInterval = 0.2f;
	public float maxSplashInterval = 0.5f;
    
    private float nextBubbleTime;
	private float nextSplashTime;
    
	protected override void Initialisation () {
	}

    protected override void UpdateLogic() {
        if ((Time.time > nextBubbleTime) && (pot.temprature >= pot.waterSimmeringPoint)) {
            float bubbleScale = (pot.temprature - pot.waterSimmeringPoint) / (pot.waterBoilingPoint - pot.waterSimmeringPoint);
            float nextBubbleInterval = 1f / Mathf.Lerp((Random.Range(minBubbleRateBoiling, maxBubbleRateBoiling) * bubbleScale),
                                                        Random.Range(minBubbleRateBoiling, maxBubbleRateBoiling), 
                                                        bubbleIntervalScaleWeight);
            nextBubbleTime = Time.time + nextBubbleInterval;
            SplashAtPoint(Random.Range(bubbleFreeBorder, pixelsX - bubbleFreeBorder),
                            Random.Range(bubbleFreeBorder, pixelsY - bubbleFreeBorder), 
                            (int)(Mathf.Lerp((float)splashForce, (float)splashForce * bubbleScale, (float)bubbleForceScaleWeight)));
        }
		if (PotIsInSkink && Time.time > nextSplashTime) {
			nextSplashTime = Time.time + Random.Range (minSplashInterval, maxSplashInterval);
			SplashAtPoint((pixelsX / 2) + Random.Range(-3, 3), (pixelsY / 2) + Random.Range(-3, 3), (int)(splashForce * faucet.TotalWaterRate));
			
		}
    }
}