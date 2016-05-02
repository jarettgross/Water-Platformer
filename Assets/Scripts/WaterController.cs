using UnityEngine;
using System.Collections;

public class WaterController : MonoBehaviour {

	private Rigidbody2D rigidBody;

	public ParticleSystem waterPack;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
		handleWater ();
	}

	private void handleWater() {
		if (Input.GetMouseButton (0)) { //left click
			Vector2 playerPos2D = new Vector2 (transform.position.x, transform.position.y);

			Vector3 worldMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector2 mouseRelativePlayer = new Vector2 (worldMousePos.x - playerPos2D.x, worldMousePos.y - playerPos2D.y);

			RaycastHit2D hit = Physics2D.Raycast (playerPos2D, mouseRelativePlayer);
			if (hit.collider != null) {
				rigidBody.AddForce (-50 * mouseRelativePlayer.normalized);
 			}
		}
	}
}
