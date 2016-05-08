using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaterController : MonoBehaviour {

	private Rigidbody2D rigidBody;

	public ParticleSystem waterPack;
	public float initialWaterAmount;
	private float waterRemaining;

	public Slider waterAmountSlider;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();

		waterRemaining = initialWaterAmount;

		waterAmountSlider.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width / 3, Screen.height / 10); //set slider width, height
	}

	void FixedUpdate() {
		handleWater ();
	}

	private void handleWater() {
		Vector2 playerPos2D = new Vector2 (transform.position.x, transform.position.y);
		Vector3 worldMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 mouseRelativePlayer = new Vector2 (worldMousePos.x - playerPos2D.x, worldMousePos.y - playerPos2D.y);

		if (waterRemaining > 0) {
			if (Input.GetMouseButtonDown (0)) { //start left clicking
				waterPack.Play ();
			}
			if (Input.GetMouseButton (0)) { //left clicking, apply force to player
				waterPack.transform.eulerAngles = new Vector3 (Mathf.Atan2 ((mouseRelativePlayer.x), (mouseRelativePlayer.y)) * Mathf.Rad2Deg - 90, 90, 0); //water angle based off mouse pos

				//force applied to the player
				Vector2 oppWaterForce = new Vector2 (-150 * mouseRelativePlayer.normalized.x, -50 * mouseRelativePlayer.normalized.y);
				rigidBody.AddForce (oppWaterForce);

				waterRemaining -= Time.deltaTime;
				waterAmountSlider.value = waterRemaining / initialWaterAmount;
				if (waterRemaining <= 0) {
					waterPack.Stop ();
				}
			}
			if (Input.GetMouseButtonUp (0)) { //stopped left clicking, end particle system
				waterPack.Stop ();
			}
		}
	}
}
