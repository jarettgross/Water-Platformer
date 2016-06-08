using UnityEngine;
using System.Collections;

public class BackgroundMusicManager : MonoBehaviour {

	private static bool startBackgroundMusic;
	private GameObject bas;
	private AudioSource bgm;

	void Awake() {
		if (!startBackgroundMusic) {
			gameObject.GetComponent<AudioSource> ().Play ();
			DontDestroyOnLoad (gameObject);
			startBackgroundMusic = false;
		}
	}

	void Start() {
		bas = GameObject.FindGameObjectWithTag ("ButtonsAndSound");
		bgm = gameObject.GetComponent<AudioSource> ();
	}

	void Update() {
		if (bas.GetComponent<ButtonAndSoundManager>().isBGMMuted && bgm.isPlaying) {
			gameObject.GetComponent<AudioSource> ().Pause ();
		} else if (!bas.GetComponent<ButtonAndSoundManager>().isBGMMuted && !bgm.isPlaying) {
			gameObject.GetComponent<AudioSource> ().UnPause ();
		}
	}
}
