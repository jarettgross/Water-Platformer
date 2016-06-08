using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InstructionTextManager : MonoBehaviour {

	public float timeUntilFade = 10.0f;
	public float fadeSpeed = 1.5f;

	private float fadeTimer;
	private Color textColor;
	private Color fadeColor;
	private bool finishedFade;

	void Start() {
		textColor = gameObject.GetComponent<Text> ().color;
		fadeColor = new Color (textColor.r, textColor.g, textColor.b, 0);
	}

	void Update() {
		if (Time.timeSinceLevelLoad > timeUntilFade && !finishedFade) {
			gameObject.GetComponent<Text> ().color = Color.Lerp (textColor, fadeColor, fadeTimer / fadeSpeed); //lerp color with alpha value to transparent
			fadeTimer += Time.deltaTime;
			if (gameObject.GetComponent<Text>().color.a == 0) {
				finishedFade = true;
			}
		}
	}
}
