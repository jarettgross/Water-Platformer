using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour {

	public Texture2D fadeTexture;
	public float fadeSpeed = 1.5f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;

	void OnGUI() {
		alpha += fadeDir * Time.deltaTime / fadeSpeed;
		alpha = Mathf.Clamp01 (alpha);
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeTexture);
	}

	public float BeginFade(int direction) {
		fadeDir = direction;
		return fadeSpeed;
	}

	public float BeginFade(float _fadeSpeed, int direction) {
		fadeDir = direction;
		fadeSpeed = _fadeSpeed;
		return fadeSpeed;
	}
}
