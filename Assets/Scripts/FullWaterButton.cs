using UnityEngine;
using System.Collections;

public class FullWaterButton : MonoBehaviour {

	/**
	 * Water button that keeps playing once the trigger is pushed. Stops when trigger is pushed agian.
	**/

	public ParticleSystem waterSystem;
	public AudioClip switchSound;
	public bool playOnStart;

	private bool isPlaying = false;

	void Start () {
		if (playOnStart) {
			waterSystem.Play ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" || other.tag == "WaterPushBlock") {
			AudioSource.PlayClipAtPoint (switchSound, transform.position);
			if (isPlaying) {
				waterSystem.Stop ();
			} else {
				waterSystem.Play ();
			}
			isPlaying = !isPlaying;
		}
	}
}
