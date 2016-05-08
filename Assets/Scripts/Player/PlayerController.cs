using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D rigidBody;

	public float movementSpeed;

	public Transform[] groundPoints; //location where player touches ground
	public float groundRadius; //radius of point for player touching ground
	private bool isGrounded;
	public float jumpForce;

	public LayerMask specifyGround;

	private bool isOnMovingPlatform;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();

		isOnMovingPlatform = false;
	}

	// Update is called once per frame
	void Update() {
		isGrounded = checkGrounded ();
		handleMovement (Input.GetAxis ("Horizontal"));
		flipHorizontal ();
	}

	//Handle left, right movement; jumping
	private void handleMovement(float horizontal) {
		rigidBody.velocity = new Vector2 (movementSpeed * horizontal, rigidBody.velocity.y); //left, right movement
		if (isOnMovingPlatform) {
			rigidBody.velocity += transform.parent.GetComponent<Rigidbody2D> ().velocity;
		}

		if (isGrounded && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) { //jumping
			isGrounded = false;
			rigidBody.AddForce (new Vector2 (0, jumpForce));
		} else {
			isGrounded = true;
		}
	}

	//Turn sprite around depending on location of mouse relative to the player
	private void flipHorizontal() {
		Vector2 playerPos2D = new Vector2 (transform.position.x, transform.position.y);
		Vector3 worldMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 mouseRelativePlayer = new Vector2 (worldMousePos.x - playerPos2D.x, worldMousePos.y - playerPos2D.y);
		if (mouseRelativePlayer.x < 0) { //mouse is left of player
			Vector3 invertedScale = transform.localScale;
			invertedScale.x = -1;
			transform.localScale = invertedScale;
		} else { //mouse is right of player
			Vector3 invertedScale = transform.localScale;
			invertedScale.x = 1;
			transform.localScale = invertedScale;
		}
	}

	//Check if grounded so player can jump
	private bool checkGrounded() {
		if (rigidBody.velocity.y <= 0) {
			foreach (Transform gp in groundPoints) {
				Collider2D[] colliders = Physics2D.OverlapCircleAll (gp.position, groundRadius, specifyGround);
				for (int i = 0; i < colliders.Length; i++) {
					if (colliders [i].gameObject != gameObject) { //found collision with ground, allow jumping
						return true;
					}
				}
			}
		}
		return false;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "MovingPlatform") {
			transform.parent = collider.transform;
			isOnMovingPlatform = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "MovingPlatform") {
			transform.parent = null;
			isOnMovingPlatform = false;
		}
	}
}
