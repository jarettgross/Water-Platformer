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

	private const float waterPackPos = 0.28f; //position of waterPack relative to the player
	private const float waterPackPosTop = 0.43f; //position of waterPack relative to the player

	private Vector2 oppWaterForce = new Vector2(0, 0);
	private bool isArrowKeyDown = false;
	public bool isPlayingWater = false;
	public bool canPlayWaterSound;

	public float waterIncreaseRate = 1.4f; //how quickly to refill player's water when in a puddle
	public float waterOverlapRadius = 0.07f;

	public bool manualSetWaterAmount;
	public float manualWaterAmount;

	void Start() {
		rigidBody = GetComponent<Rigidbody2D> ();

		waterRemaining = initialWaterAmount;
		if (manualSetWaterAmount) {
			waterRemaining = manualWaterAmount;
		}

		waterAmountSlider.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width / 3, Screen.height / 10); //set slider width, height
	}

	void Update() {
		handleWater ();
		waterAmountSlider.GetComponentInChildren<Text> ().text = Mathf.Ceil(waterRemaining).ToString ();
	}

	void FixedUpdate() {
		if (waterRemaining > 0 && isArrowKeyDown) {
			rigidBody.AddForce (oppWaterForce);
		}
	}

	private void handleWater() {
		if (!gameObject.GetComponent<PlayerController>().isInHeatArea && waterRemaining > 0) {

			//prevent water from going through objects when the waterPack gameobject is overlapping another object
			bool isOverlappingObj = false;
			Collider2D[] colliders = Physics2D.OverlapCircleAll (waterPack.transform.position, waterOverlapRadius);
			for (int i = 0; i < colliders.Length; i++) {
				if (colliders [i].gameObject != gameObject && colliders [i].gameObject != waterPack.gameObject
					&& colliders[i].gameObject.GetComponent<BubbleMaker>() == null && colliders[i].gameObject.tag != "Puddle"
					&& !gameObject.GetComponent<PlayerController>().isInSnowArea) {
					isOverlappingObj = true;
					isPlayingWater = false;
					canPlayWaterSound = true;
					waterPack.Stop ();
				}
			}

			//shoot water in the direction specified by arrows keys
			if (GetComponent<PlayerController>().hInput.GetKeyDown("shoot_right")) {
				isArrowKeyDown = true;
				waterPack.transform.eulerAngles = new Vector3 (0, 90, 0); //set water angle
				waterPack.transform.localPosition = new Vector3 (waterPackPos, 0, 0); //set water location relative to player
				if (!isOverlappingObj) waterPack.Play ();
				oppWaterForce = new Vector2 (-HORIZ_FORCE, 0); //set player force from water

			} else if (GetComponent<PlayerController>().hInput.GetKeyDown("shoot_left")) {
				isArrowKeyDown = true;
				waterPack.transform.eulerAngles = new Vector3 (0, -90, 0);
				waterPack.transform.localPosition = new Vector3 (waterPackPos, 0, 0);
				if (!isOverlappingObj) waterPack.Play ();
				oppWaterForce = new Vector2 (HORIZ_FORCE, 0);

			} else if (GetComponent<PlayerController>().hInput.GetKeyDown("shoot_up")) {
				isArrowKeyDown = true;
				waterPack.transform.eulerAngles = new Vector3 (-90, 90, 0);
				waterPack.transform.localPosition = new Vector3 (0, waterPackPosTop, 0);
				if (!isOverlappingObj) waterPack.Play ();
				oppWaterForce = new Vector2 (0, -VERTICAL_FORCE);
			}

			if (GetComponent<PlayerController>().hInput.GetKey("shoot_right") || GetComponent<PlayerController>().hInput.GetKey("shoot_left") || GetComponent<PlayerController>().hInput.GetKey("shoot_up")) { //using arrows, lower water amount
				if (!isOverlappingObj && !isPlayingWater) { //play water if not already
					waterPack.Play ();
					isPlayingWater = true;
					canPlayWaterSound = true;
				}

				waterRemaining -= Time.deltaTime;
				waterAmountSlider.value = waterRemaining / initialWaterAmount;

				if (waterRemaining <= 0) {
					isPlayingWater = false;
					canPlayWaterSound = false;
					waterPack.Stop ();
				}
			}
		} else {
			oppWaterForce = new Vector2 (0, 0);
			waterPack.Stop ();
			isPlayingWater = false;
			canPlayWaterSound = false;
		}

		if (!(GetComponent<PlayerController>().hInput.GetKey("shoot_right") || GetComponent<PlayerController>().hInput.GetKey("shoot_left") || GetComponent<PlayerController>().hInput.GetKey("shoot_up"))) { //not using arrows, stop water animation
			isArrowKeyDown = false;
			isPlayingWater = false;
			canPlayWaterSound = false;
			waterPack.Stop ();
		}
	}

	//player went into a puddle, increase player water
	public void IncreaseWaterAmount() {
		waterRemaining += (Time.deltaTime / waterIncreaseRate);
		waterAmountSlider.value = waterRemaining / initialWaterAmount;
	}
}
