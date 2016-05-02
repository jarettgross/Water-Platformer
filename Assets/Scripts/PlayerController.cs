using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D rigidBody;

	public float movementSpeed;
	private bool facingRight;

	public Transform groundPoint; //location where player touches ground
	public float groundRadius; //radius of point for player touching ground
	private bool grounded;
	private bool isJumping;
	public float jumpForce;

	public LayerMask specifyGround;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		facingRight = true;
	}

	// Update is called once per frame
	void Update() {
		handleInput ();
	}

	void FixedUpdate () {
		grounded = isGrounded ();
		float horizontal = Input.GetAxis ("Horizontal");

		handleMovement (horizontal);
		flipHorizontal (horizontal);
	}

	//Handle left, right movement; jumping
	private void handleMovement(float horizontal) {
		rigidBody.velocity = new Vector2 (movementSpeed * horizontal, rigidBody.velocity.y);
		if (grounded && isJumping) {
			grounded = false;
			rigidBody.AddForce (new Vector2 (0, jumpForce));

		} else {
			grounded = true;
		}
	}

	private void handleInput() {
		if (Input.GetButtonDown("Jump")) {
			isJumping = true;
		}
		if (Input.GetButtonUp("Jump")) {
			isJumping = false;
		}
	}

	//Turn sprite around
	private void flipHorizontal(float horizontal) {
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			facingRight = !facingRight;
			Vector3 invertedScale = transform.localScale;
			invertedScale.x *= -1;
			transform.localScale = invertedScale;
		}
	}

	//Check if grounded so player can jump
	private bool isGrounded() {
		if (rigidBody.velocity.y <= 0) {
			Collider2D[] colliders = Physics2D.OverlapCircleAll (groundPoint.position, groundRadius, specifyGround);
			for (int i = 0; i < colliders.Length; i++) {
				if (colliders[i].gameObject != gameObject) {
					return true;
				}
			}
		}
		return false;
	}
}
