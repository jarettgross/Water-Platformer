using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
	public bool isInSnowArea;
	private bool isOnIceBlock;
	private bool isOnPushBlock;

	public bool preventMovement;
	private bool isRestarting;

	public ButtonAndSoundManager bsm;

	void Start() {
		rigidBody = GetComponent<Rigidbody2D> ();
		isOnMovingPlatform = false;
		isOnBubble = false;
		isInHeatArea = false;
		isOnIceBlock = false;
	}


	void Update() {
		if (Input.GetKeyDown(KeyCode.R)) {
			isRestarting = true;
		}

		if (!preventMovement && !isRestarting) {
			isGrounded = checkGrounded ();
			handleMovement (Input.GetAxis ("Horizontal"));
			flipHorizontal ();
		} else {
			rigidBody.velocity = new Vector2 (0, 0);
			rigidBody.isKinematic = true;
			if (isRestarting) {
				StartCoroutine (RestartLevel ());
			}
		}
	}

    //Handle left, right movement; jumping
    private void handleMovement(float horizontal) {
		if (!isOnIceBlock) {
        	rigidBody.velocity = new Vector2(movementSpeed * horizontal, rigidBody.velocity.y); //left, right movement

	        //inherit velocity from parent
			if (transform.parent != null && (isOnMovingPlatform || isOnPushBlock)) {
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
            curVel = Vector2.Lerp(rigidBody.velocity, vel, 3*Time.deltaTime);
            if (isGrounded && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W))) { //jumping
                isGrounded = false;
                rigidBody.AddForce(new Vector2(0, jumpForce));
            } else {
                isGrounded = true;
            }

            if (Input.GetKey(KeyCode.D)) {
				rigidBody.velocity = new Vector2(movementSpeed * horizontal, rigidBody.velocity.y);
            }

            if (Input.GetKey(KeyCode.A)) {
				rigidBody.velocity = new Vector2(movementSpeed * horizontal, rigidBody.velocity.y);
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow)) {
                rigidBody.velocity = new Vector2(movementSpeed * horizontal, rigidBody.velocity.y);
            }
		}
	}

	//Turn sprite around depending on arrow key pressed
	private void flipHorizontal() {
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			if (transform.localScale.x > 0) {
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			if (transform.localScale.x < 0) {
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		}
	}

	//Check if grounded so player can jump
	private bool checkGrounded() {
		if (isOnMovingPlatform || isOnPushBlock || rigidBody.velocity.y <= 0.001f) {
			
			//Middle ground point
			Collider2D[] colliders = Physics2D.OverlapAreaAll (new Vector2 (groundPoints [0].position.x - groundRadius, groundPoints [0].position.y), 
				new Vector2 (groundPoints [0].position.x + groundRadius, groundPoints [0].position.y - groundRadius), specifyGround);

			for (int i = 0; i < colliders.Length; i++) {
				if (colliders [i].gameObject != gameObject) { //found collision with ground, allow jumping
					if (colliders[i].tag == "IceBlock") isOnIceBlock = true;
					return true;
				}
			}

			//Right ground point
			colliders = Physics2D.OverlapAreaAll (new Vector2 (groundPoints [1].position.x, groundPoints [1].position.y), 
				new Vector2 (groundPoints [1].position.x - groundRadius, groundPoints [1].position.y - groundRadius), specifyGround);

			for (int i = 0; i < colliders.Length; i++) {
				if (colliders [i].gameObject != gameObject) {
					if (colliders[i].tag == "IceBlock") isOnIceBlock = true;
					return true;
				}
			}

			//Left ground point
			colliders = Physics2D.OverlapAreaAll (new Vector2 (groundPoints [2].position.x, groundPoints [2].position.y), 
				new Vector2 (groundPoints [2].position.x + groundRadius, groundPoints [2].position.y - groundRadius), specifyGround);

			for (int i = 0; i < colliders.Length; i++) {
				if (colliders [i].gameObject != gameObject) {
					if (colliders[i].tag == "IceBlock") isOnIceBlock = true;
					return true;
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
		} else if (collider.tag == "WaterPushBlock") {
			isOnPushBlock = true;
			if (transform.parent == null && collider.gameObject.transform.parent != null) {
				transform.parent = collider.gameObject.transform;
				GetComponent<BoxCollider2D> ().sharedMaterial.friction = 0;
			}
		} else if (collider.tag == "Snow") {
			isInSnowArea = true;
		} else if (collider.tag == "Spikes") {
			preventMovement = true;
			StartCoroutine (RestartLevel ());
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
		} else if (collider.tag == "WaterPushBlock") {
			isOnPushBlock = false;
			if (transform.parent == collider.gameObject.transform) {
				transform.parent = null;
			}
		} else if (collider.tag == "Snow") {
			isInSnowArea = false;
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

	IEnumerator RestartLevel() {
		float fadeTime = GameObject.Find ("FadeManager").GetComponent<FadeManager> ().BeginFade (1, 1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene(SceneManager.GetActiveScene ().name);
	}
}
