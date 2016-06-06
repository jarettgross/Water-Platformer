using UnityEngine;
using System.Collections;

public class PushBlock : MonoBehaviour {

	/**
	 * Block that can be pushed by the player object
	**/

	public float frictionAmount = 0.75f;
	private float originalFriction;

	void Start() {
		originalFriction = frictionAmount;
	}

	void FixedUpdate() {
		//decrease velocity over time
		Vector2 tempVelocity = new Vector2 (gameObject.GetComponent<Rigidbody2D> ().velocity.x * frictionAmount, gameObject.GetComponent<Rigidbody2D> ().velocity.y);
		gameObject.GetComponent<Rigidbody2D> ().velocity = tempVelocity;
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Bubble") {
			GetComponent<Rigidbody2D> ().freezeRotation = false;
			frictionAmount = 1.0f;
		} else if (col.gameObject.tag == "MovingPlatform") {
			frictionAmount = 1.0f;
		}
	}

	void OnCollisionExit2D(Collision2D col) {
		if (col.gameObject.tag == "Bubble" || col.gameObject.tag == "MovingPlatform") {
			frictionAmount = originalFriction;
		}
	}
}
