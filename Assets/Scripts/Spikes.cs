using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Spikes : MonoBehaviour {

	private float fadeSpeed = 1.0f;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			other.GetComponent<PlayerController> ().preventMovement = true;
			StartCoroutine (ReloadLevel ());
		}
	}

	IEnumerator ReloadLevel() {
		float fadeTime = GameObject.Find ("FadeManager").GetComponent<FadeManager> ().BeginFade (fadeSpeed, 1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene(SceneManager.GetActiveScene ().name);
	}
}
