using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleMaker : MonoBehaviour {

	/**
	 * An object which, when hit with water, creates a bubble that the player can jump on
	**/

	public GameObject bubble;

	public float waitTime; //how long until another bubble can be created
	private float currentTime;

	//speed of the bubble
	public float velocityX;
	public float velocityY;

	public float maxBubbleHeight; //how high a bubble can reach before it pops
	public float maxBubbleWidth; //how far (x direction) a bubble can travel before reversing

	public bool isBubbleActive = false;

	public List<GameObject> bubbleList;

	public ButtonAndSoundManager bsm;

	void Update () {
		foreach (GameObject b in bubbleList) {
			if (Mathf.Abs(b.transform.position.y - transform.position.y) >= maxBubbleHeight) { //then destroy bubble
				if (b.transform.childCount > 0) { //remove children
					b.transform.GetChild (0).GetComponent<PlayerController> ().isOnMovingPlatform = false;
					b.transform.GetChild (0).GetComponent<PlayerController> ().isOnBubble = false;
					b.transform.DetachChildren ();
				}
				bubbleList.Remove (b);
				GameObject bubblePS = (GameObject) Instantiate (b.GetComponent<Bubble> ().bubblePop.gameObject, b.transform.position, Quaternion.identity); //bubble popped, play water particle system
				if (!bsm.isSFXMuted) {
					AudioSource.PlayClipAtPoint (b.GetComponent<Bubble> ().bubblePopSound, b.transform.position);
				}
				Destroy(b);
				Destroy (bubblePS, bubblePS.GetComponent<ParticleSystem> ().startLifetime);
				break;
			}

			//move bubble left and right over time
			if (b.transform.position.x - gameObject.transform.position.x >= maxBubbleWidth && b.GetComponent<Rigidbody2D> ().velocity.x > 0) {
				Vector2 tempVelocity = b.GetComponent<Rigidbody2D> ().velocity;
				b.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-tempVelocity.x, tempVelocity.y);
			} else if (b.transform.position.x - gameObject.transform.position.x <= -maxBubbleWidth && b.GetComponent<Rigidbody2D> ().velocity.x < 0) {
				Vector2 tempVelocity = b.GetComponent<Rigidbody2D> ().velocity;
				b.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-tempVelocity.x, tempVelocity.y);
			}
		}

		//time before another bubble can be created
		if (isBubbleActive) {
			currentTime += Time.deltaTime;
			if (currentTime >= waitTime) {
				isBubbleActive = false;
				currentTime = 0;
			}
		}
	}
}