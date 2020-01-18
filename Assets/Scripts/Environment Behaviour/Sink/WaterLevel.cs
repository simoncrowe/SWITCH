using UnityEngine;
using System.Collections;

public class WaterLevel : MonoBehaviour {

	public Faucet faucet;
	public Plug plug;
	// All volumes in cubic metres
	public float waterLevel = 0;
	public float pluggedDrainPerSecond =  0.00002f;
	public float unpluggedDrainPerSecond = 0.00015f;
	public float totalVolume = 0.027038793f;
	public float startingOccupiedVolume = 0.0001f;
	public float VolumeOccupied { get; private set;}
	public bool FilledByFaucet { get; set;}
	private bool overflowing = false;

	void Start () {
		VolumeOccupied = startingOccupiedVolume;
		FilledByFaucet = true;
	}

	public bool TryRemoveWater(float volume) {
		if ((VolumeOccupied - volume) > 0) {
			VolumeOccupied -= volume;
			return true;
		} else {
			return false;
		}
	}

	void Update () {
		if (FilledByFaucet) {
			if (plug.plugged) {
				// Due to time constraints, plugged now means partially blocked
				VolumeOccupied += faucet.TotalWaterRateCuM * Time.deltaTime;
			} else {
				VolumeOccupied += faucet.TotalWaterRateCuM  * Time.deltaTime;
			}
		}
		if (waterLevel > 0) {
			if (plug.plugged) {
				// Due to time constraints, plugged now means partially blocked
				VolumeOccupied -= pluggedDrainPerSecond * Time.deltaTime;
			} else {
				VolumeOccupied -= unpluggedDrainPerSecond * Time.deltaTime;
			}
		}

		waterLevel = VolumeOccupied / totalVolume; //0.027038793 cubic meter capacity
		if (waterLevel >= 1) {
			// Unused (FOR NOW!)
			overflowing = true;
			waterLevel = 1;
		}
		else {
			overflowing = false;	
		}
		transform.position = new Vector3 (-0.4565226f,0.6925f + (0.1611506f * waterLevel),-2.236148f);
		transform.localScale = new Vector3  (1f+(0.1f*waterLevel),1,1f+(0.15f * waterLevel));
	}
}