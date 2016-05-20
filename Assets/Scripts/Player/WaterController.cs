using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaterController : MonoBehaviour {

	private Rigidbody2D rigidBody;

	public ParticleSystem waterPack;
	public float initialWaterAmount;
	public float waterRemaining;

	public Slider waterAmountSlider;

	//force applied to player from waterPack
	private const int HORIZ_FORCE = 150;
	private const int VERTICAL_FORCE = 50;

	private const float waterPackPos = 0.53f; //position of waterPack relative to the player

	private Vector2 oppWaterForce = new Vector2(0, 0);
	private bool isArrowKeyDown = false;
	private bool isPlayingWater = false;

	public float waterIncreaseRate = 2; //how quickly to refill player's water when in a puddle

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();

		waterRemaining = initialWaterAmount;

		waterAmountSlider.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width / 3, Screen.height / 10); //set slider width, height
	}

	void Update() {
		handleWater ();
	}

	void FixedUpdate() {
		if (waterRemaining > 0 && isArrowKeyDown) {
			rigidBody.AddForce (oppWaterForce); //add force to player if using water
		}
	}

	private void handleWater() {
		if (!gameObject.GetComponent<PlayerController>().isInHeatArea && waterRemaining > 0) { //not in heat and has water

			//prevent water from going through objects when the waterPack gameobject is overlapping another object
			bool isOverlappingObj = false;
			Collider2D[] colliders = Physics2D.OverlapCircleAll (waterPack.transform.position, .07f);
			for (int i = 0; i < colliders.Length; i++) {
				if (colliders [i].gameObject != gameObject && colliders [i].gameObject != waterPack.gameObject) { //found collision with ground, allow jumping
					isOverlappingObj = true;
					isPlayingWater = false;
					waterPack.Stop ();
				}
			}

			//shoot water in the direction specified by arrows keys
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				isArrowKeyDown = true;
				waterPack.transform.eulerAngles = new Vector3 (0, 90, 0); //set water angle
				waterPack.transform.localPosition = new Vector3 (waterPackPos, 0, 0); //set water location relative to player
				if (!isOverlappingObj) waterPack.Play ();
				oppWaterForce = new Vector2 (-HORIZ_FORCE, 0); //set player force from water

			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				isArrowKeyDown = true;
				waterPack.transform.eulerAngles = new Vector3 (0, -90, 0);
				waterPack.transform.localPosition = new Vector3 (waterPackPos, 0, 0);
				if (!isOverlappingObj) waterPack.Play ();
				oppWaterForce = new Vector2 (HORIZ_FORCE, 0);

			} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
				isArrowKeyDown = true;
				waterPack.transform.eulerAngles = new Vector3 (-90, 90, 0);
				waterPack.transform.localPosition = new Vector3 (0, waterPackPos, 0);
				if (!isOverlappingObj) waterPack.Play ();
				oppWaterForce = new Vector2 (0, -VERTICAL_FORCE);

			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
//				isArrowKeyDown = true;
//				waterPack.transform.eulerAngles = new Vector3(90, 90, 0);
//				waterPack.transform.localPosition = new Vector3 (0, -waterPackPos, 0);
//				waterPack.Play ();
//				oppWaterForce = new Vector2 (0, VERTICAL_FORCE);
			}

			if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow)) { //using arrows, lower water amount
				if (!isOverlappingObj && !isPlayingWater) { //play water if not already
					waterPack.Play ();
					isPlayingWater = true;
				}

				waterRemaining -= Time.deltaTime; //decrease water amount
				waterAmountSlider.value = waterRemaining / initialWaterAmount;

				if (waterRemaining <= 0) { //stop water animation when none left
					isPlayingWater = false;
					waterPack.Stop ();
				}
			}
		}

		if (!(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow))) { //not using arrows, stop water animation
			isArrowKeyDown = false;
			isPlayingWater = false;
			waterPack.Stop ();
		}
	}

	//player went into a puddle, increase player water
	public void IncreaseWaterAmount() {
		waterRemaining += (Time.deltaTime / waterIncreaseRate);
		waterAmountSlider.value = waterRemaining / initialWaterAmount;
	}
}
