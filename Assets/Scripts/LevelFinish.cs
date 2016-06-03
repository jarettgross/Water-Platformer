using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour {

	public int nextLevel;
	private bool isLevelFinished;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			if (nextLevel - 1 > PlayerPrefs.GetInt ("LatestLevelBeat")) {
				PlayerPrefs.SetInt ("LatestLevelBeat", nextLevel - 1);
				PlayerPrefs.Save ();
			}

			other.GetComponent<PlayerController> ().preventMovement = true;
			StartCoroutine (ChangeLevel ());
		}
	}

	IEnumerator ChangeLevel() {
		float fadeTime = GameObject.Find ("FadeManager").GetComponent<FadeManager> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene ("Level " + nextLevel);
	}
}
