﻿using UnityEngine;
using System.Collections;

public class FullWaterButton : MonoBehaviour {

	/**
	 * Water button that keeps playing once the trigger is pushed. Stops when trigger is pushed agian.
	**/

	public ParticleSystem waterSystem;
	public AudioClip switchSound;
	public bool playOnStart;

	public bool isPlaying = false;

	public ButtonAndSoundManager bsm;

	public Sprite buttonUp;
	public Sprite buttonDown;

	void Start() {
		if (playOnStart) {
			isPlaying = true;
			waterSystem.Play ();
		}
		waterSystem.GetComponent<WaterActions> ().SetWaterTrigger (gameObject);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" || other.tag == "WaterPushBlock") {
			if (!bsm.isSFXMuted) {
				AudioSource.PlayClipAtPoint (switchSound, transform.position);
			}
			if (isPlaying) {
				waterSystem.Stop ();
				GetComponent<SpriteRenderer> ().sprite = buttonUp;
			} else {
				waterSystem.Play ();
				GetComponent<SpriteRenderer> ().sprite = buttonDown;
			}
			isPlaying = !isPlaying;
		}
	}
}
