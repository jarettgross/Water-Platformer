using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

	/**
	 * A bubble that the player can jump on. Exists until it reaches a certain height, then pops
	**/

	public ParticleSystem bubblePop;
	public AudioClip bubbleFormSound;
	public AudioClip bubblePopSound;

	void Start () {
		AudioSource.PlayClipAtPoint (bubbleFormSound, transform.position);
	}
}
