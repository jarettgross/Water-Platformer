using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour {

	public float dampTime;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	void FixedUpdate () {
		if (target) { //player is not null
			Vector3 point = Camera.main.WorldToViewportPoint (target.position);
			Vector3 delta = target.position - Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, dampTime);
		}
	}
}
