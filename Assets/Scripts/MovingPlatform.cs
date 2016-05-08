﻿using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	private Rigidbody2D rb;
	private BoxCollider2D topTrigger;

	public float horizontalSpeed;
	public float verticalSpeed;
	public float distance;
	public float waitTime;

	private Vector2 startPos;
	private bool isCycleFinished;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		startPos.x = transform.position.x;
		startPos.y = transform.position.y;

		isCycleFinished = true;
	}

	void FixedUpdate() {
		if (isCycleFinished) {
			StartCoroutine (handlePlatformMove ());
		}
	}

	IEnumerator handlePlatformMove() {
		isCycleFinished = false;

		rb.velocity = new Vector2 (horizontalSpeed, verticalSpeed); //move in forward direction
		while (Mathf.Sqrt (Mathf.Pow (transform.position.x - startPos.x, 2) + Mathf.Pow (transform.position.y - startPos.y, 2)) < distance) { //wait while platform has not moved predefined distance
			yield return null;
		}

		rb.velocity = new Vector2 (0, 0); //stop moving
		yield return new WaitForSeconds (waitTime); //wait when platform reached predefined distance
		rb.velocity = new Vector2 (-horizontalSpeed, -verticalSpeed); //move in reverse direction

		while (!Mathf.Approximately(Mathf.Sqrt (Mathf.Pow (transform.position.x - startPos.x, 2) + Mathf.Pow (transform.position.y - startPos.y, 2)), 0)) { //wait while platform has not moved predefined distance
			if (transform.position.x <= startPos.x && transform.position.y <= startPos.y) {
				break;
			}
			yield return null;
		}

		rb.velocity = new Vector2 (0, 0); //stop moving
		yield return new WaitForSeconds (waitTime); //wait when platform reached predefined distance

		isCycleFinished = true;
	}
}
