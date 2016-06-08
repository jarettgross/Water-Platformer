﻿using UnityEngine;
using System.Collections;

public class SquirtWaterButton : MonoBehaviour {

	/**
	 * Water button that plays a squirt of water when the trigger is pushed.
	**/

	public ParticleSystem waterSystem;
	public AudioClip switchSound;

	public bool isPlaying = false;
	private float timer = 0.0f;
	public float playTime = 1.0f;

	public ButtonAndSoundManager bsm;

	void Update() {
		if (isPlaying) {
			timer += Time.deltaTime;
			if (timer >= playTime) { //play time over, restart timer
				waterSystem.Stop ();
				isPlaying = false;
				timer = 0.0f;
			}
		}
		waterSystem.GetComponent<WaterActions> ().SetWaterTrigger (gameObject);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" || other.tag == "WaterPushBlock") {
			if (!bsm.isSFXMuted) {
				AudioSource.PlayClipAtPoint (switchSound, transform.position);
			}
			waterSystem.Play ();
			isPlaying = true;
		}
	}
}
