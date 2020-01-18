using UnityEngine;

public class StoveAudioL : MonoBehaviour{
	
	public AudioClip ignitionSound;
	
	public void ChangeState (int knobState){
		if (knobState == 0) {
			GetComponent<AudioSource>().Stop();
		} else if (knobState == 1) {
			GetComponent<AudioSource>().PlayOneShot(ignitionSound);
			GetComponent<AudioSource>().volume = .15f;
			GetComponent<AudioSource>().Play();
		} else if (knobState == 2) {
			GetComponent<AudioSource>().volume = .3f;
		} else if (knobState == 3) {
			GetComponent<AudioSource>().volume = .45f;
		} else if (knobState == 4) {
			GetComponent<AudioSource>().volume = .6f;
		} else if (knobState == 5) {
			GetComponent<AudioSource>().volume = .75f;
		} else{
			GetComponent<AudioSource>().volume = .9f;
		}
	}
}