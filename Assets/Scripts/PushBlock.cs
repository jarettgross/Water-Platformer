using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PushBlock : MonoBehaviour {

	/**
	 * Block that can be pushed by the player object
	**/

	public float frictionAmount = 0.75f;
	private float originalFriction;

	private List<GameObject> bubbles;

	void Start() {
		bubbles = new List<GameObject> ();
		originalFriction = frictionAmount;
	}

	void Update() {
		if (bubbles.Count > 0) {
			foreach (GameObject b in bubbles) {
				if (b == null) {
					bubbles.Remove (b);
					if (bubbles.Count == 0) {
						frictionAmount = originalFriction;
						transform.parent = null;
					}
					break;
				}
			}
		}
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
			bubbles.Add (col.gameObject);
			transform.parent = col.gameObject.transform;
		} else if (col.gameObject.tag == "MovingPlatform") {
			frictionAmount = 1.0f;
			transform.parent = col.gameObject.transform;
		}
	}

	void OnCollisionExit2D(Collision2D col) {
		if (col.gameObject.tag == "Bubble" || col.gameObject.tag == "MovingPlatform") {
			frictionAmount = originalFriction;
			if (transform.parent == col.gameObject.transform) {
				transform.parent = null;
			}
		}
	}
}
