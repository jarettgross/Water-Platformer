using UnityEngine;
using System.Collections;

public class BackgroundMusicManager : MonoBehaviour {

	private static bool startBackgroundMusic;
	private GameObject bas;
	private AudioSource bgm;

	void Awake() {
		gameObject.GetComponent<AudioSource> ().Play ();
		DontDestroyOnLoad (gameObject);
	}

	void Start() {
		bgm = gameObject.GetComponent<AudioSource> ();
	}

	void OnLevelWasLoaded(int level) {
		bas = GameObject.FindGameObjectWithTag ("ButtonsAndSound");
	}

	void Update() {
		if (bas != null) {
			if (bas.GetComponent<ButtonAndSoundManager> ().isBGMMuted && bgm.isPlaying) {
				gameObject.GetComponent<AudioSource> ().Pause ();
			} else if (!bas.GetComponent<ButtonAndSoundManager> ().isBGMMuted && !bgm.isPlaying) {
				gameObject.GetComponent<AudioSource> ().UnPause ();
			}
		}
	}
}
