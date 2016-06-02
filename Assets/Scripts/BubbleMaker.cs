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


	void Update () {
		foreach (GameObject b in bubbleList) {
			if (b.transform.position.y >= maxBubbleHeight) { //then destroy bubble
				if (b.transform.childCount > 0) { //remove children
					b.transform.GetChild (0).GetComponent<PlayerController> ().isOnMovingPlatform = false;
					b.transform.GetChild (0).GetComponent<PlayerController> ().isOnBubble = false;
					b.transform.DetachChildren ();
				}
				bubbleList.Remove (b);
				GameObject waterPS = (GameObject) Instantiate (b.GetComponent<Bubble> ().bubblePop.gameObject, b.transform.position, Quaternion.identity); //bubble popped, play water particle system
				Destroy(b);
				Destroy (waterPS, waterPS.GetComponent<ParticleSystem> ().startLifetime);
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