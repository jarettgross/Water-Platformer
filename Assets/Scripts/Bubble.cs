using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

	/**
	 * A bubble that the player can jump on. Exists until it reaches a certain height, then pops
	**/

	public ParticleSystem bubblePop;
	public AudioClip bubbleFormSound;
	public AudioClip bubblePopSound;

	public BubbleMaker bubbleMakerParent;

	private Vector3 center;
	private Vector3 extents;
	private Collider2D[] colliders;
	private float delta = .15f;

	void Start () {
		AudioSource.PlayClipAtPoint (bubbleFormSound, transform.position);
	}

	void Update() {
		//if the player or a pushBlock is inside the bubble, then pop the bubble
		center = gameObject.GetComponent<BoxCollider2D> ().bounds.center;
		extents = gameObject.GetComponent<BoxCollider2D> ().bounds.extents;
		colliders = Physics2D.OverlapAreaAll (new Vector2 (center.x - extents.x + delta, center.y + extents.y - delta), new Vector2 (center.x + extents.x - delta, center.y - extents.y + delta));
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders [i].tag == "Player" || colliders[i].tag == "WaterPushBlock") {
				if (transform.childCount > 0) { //remove children
					transform.GetChild (0).GetComponent<PlayerController> ().isOnMovingPlatform = false;
					transform.GetChild (0).GetComponent<PlayerController> ().isOnBubble = false;
					transform.DetachChildren ();
				}
				bubbleMakerParent.bubbleList.Remove (gameObject);

				GameObject bubblePS = (GameObject) Instantiate (gameObject.GetComponent<Bubble> ().bubblePop.gameObject, gameObject.transform.position, Quaternion.identity); //bubble popped, play water particle system
				AudioSource.PlayClipAtPoint (gameObject.GetComponent<Bubble>().bubblePopSound, gameObject.transform.position);
				Destroy (bubblePS, bubblePS.GetComponent<ParticleSystem> ().startLifetime);
				Destroy (gameObject);
				break;
			}
		}
	}
}
