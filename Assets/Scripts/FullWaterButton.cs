using UnityEngine;
using System.Collections;

public class FullWaterButton : MonoBehaviour {

	/**
	 * Water button that keeps playing once the trigger is pushed. Stops when trigger is pushed agian.
	**/

	public ParticleSystem waterSystem;
	public bool playOnStart;

	private bool isPlaying = false;

	// Use this for initialization
	void Start () {
		if (playOnStart) {
			waterSystem.Play ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" || other.tag == "WaterPushBlock") {
			if (isPlaying) {
				waterSystem.Stop ();
			} else {
				waterSystem.Play ();
			}
			isPlaying = !isPlaying;
		}
	}
}
