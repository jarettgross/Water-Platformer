using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	/**
	 * Controls player movement and some player-object interactions
	**/

	private Rigidbody2D rigidBody;

	public float movementSpeed;

	public Transform[] groundPoints; //location where player touches ground
	public float groundRadius; //radius of point for player touching ground
	private bool isGrounded;
	public float jumpForce;

    public Vector2 curVel;

	public LayerMask specifyGround;

	public bool isOnMovingPlatform;
	public bool isOnBubble;
	public bool isInHeatArea;
	private bool isOnIceBlock;

	public Vector3 resetPosition;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		isOnMovingPlatform = false;
		isOnBubble = false;
		isInHeatArea = false;
		isOnIceBlock = false;
	}

	// Update is called once per frame
	void Update() {
		isGrounded = checkGrounded ();
		handleMovement (Input.GetAxis ("Horizontal"));
		flipHorizontal ();
	}

    //Handle left, right movement; jumping
    private void handleMovement(float horizontal) {
		if (!isOnIceBlock) {
        	rigidBody.velocity = new Vector2(movementSpeed * horizontal, rigidBody.velocity.y); //left, right movement

	        //inherit velocity from parent
	        if (isOnMovingPlatform) {
	            if (isOnBubble) {
	                rigidBody.velocity += new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x, 0); //only inherit x velocity from bubble
	            } else {
	                rigidBody.velocity += transform.parent.GetComponent<Rigidbody2D>().velocity; //inherit x, y velocity from moving platform
	            }
	        }

	        if (isGrounded && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W))) { //jumping
	            isGrounded = false;
	            rigidBody.AddForce(new Vector2(0, jumpForce));
	        } else {
	            isGrounded = true;
	        }
		} else if (isOnIceBlock) {
            Vector2 vel = new Vector2(movementSpeed * horizontal, rigidBody.velocity.y);
            curVel = Vector2.Lerp(curVel, vel, 3*Time.deltaTime);
            if (isGrounded && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W))) { //jumping
                isGrounded = false;
                rigidBody.AddForce(new Vector2(0, jumpForce));
            } else {
                isGrounded = true;
            }

            if (Input.GetKey(KeyCode.D)) {
                rigidBody.velocity = curVel;
            }

            if (Input.GetKey(KeyCode.A)) {
                rigidBody.velocity = curVel;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow)) {
                rigidBody.velocity = new Vector2(movementSpeed * horizontal, rigidBody.velocity.y);
            }
		}
	}

	//Turn sprite around depending on arrow key pressed
	private void flipHorizontal() {
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			Vector3 invertedScale = transform.localScale;
			invertedScale.x = -1;
			transform.localScale = invertedScale;
		} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			Vector3 invertedScale = transform.localScale;
			invertedScale.x = 1;
			transform.localScale = invertedScale;
		}
	}

	//Check if grounded so player can jump
	private bool checkGrounded() {
		if (isOnMovingPlatform || rigidBody.velocity.y <= 0.001f) {
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
		if (collider.tag == "MovingPlatform") { //on moving platform, make platform parent so player moves with platform
			transform.parent = collider.transform;
			isOnMovingPlatform = true; //for altering player velocity
			isOnBubble = true;
		} else if (collider.tag == "Heat") { //in heat area, can't use water
			isInHeatArea = true;
		} else if (collider.tag == "Bubble") { //on bubble, make bubble parent so player moves with bubble
			transform.parent = collider.transform;
			isOnMovingPlatform = true;
			isOnBubble = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "MovingPlatform") { //left moving platform, player now has no parent
			transform.parent = null;
			isOnMovingPlatform = false;
		} else if (collider.tag == "Heat") { //left heat area, can use water
			isInHeatArea = false;
		} else if (collider.tag == "Bubble") { //left bubble, player now has no parent
			transform.parent = null;
			isOnMovingPlatform = false;
			isOnBubble = false;
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "IceBlock") {
			isOnIceBlock = true;
            GetComponent<Collider2D>().sharedMaterial.friction = 0;
		}
	}

	void OnCollisionExit2D(Collision2D col) {
		if (col.gameObject.tag == "IceBlock") {
			isOnIceBlock = false;
		}
	}
}
