﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonAndSoundManager : MonoBehaviour {

	public bool isSFXMuted;
	public GameObject sfxButton;
	public Sprite sfxOnSprite;
	public Sprite sfxOffSprite;

	public bool isBGMMuted;
	public GameObject bgmButton;
	public Sprite bgmOnSprite;
	public Sprite bgmOffSprite;

	void Awake() {
		if (PlayerPrefs.GetInt("SFXMute") == 1) {
			MuteSoundEffects ();
		}
		if (PlayerPrefs.GetInt("BGMMute") == 1) {
			MuteBackgroundMusic ();
		}
	}

	void Start() {
		if (isBGMMuted) {
			bgmButton.GetComponent<Image> ().sprite = bgmOffSprite;
		} else {
			bgmButton.GetComponent<Image> ().sprite = bgmOnSprite;
		}

		if (isSFXMuted) {
			sfxButton.GetComponent<Image> ().sprite = sfxOffSprite;
		} else {
			sfxButton.GetComponent<Image> ().sprite = sfxOnSprite;
		}
	}

	public void GoToLevelSelect() {
		PlayerPrefs.Save ();
		SceneManager.LoadScene ("Level Select");
	}

	public void MuteBackgroundMusic() {
		isBGMMuted = !isBGMMuted;
		if (isBGMMuted) {
			bgmButton.GetComponent<Image> ().sprite = bgmOffSprite;
		} else {
			bgmButton.GetComponent<Image> ().sprite = bgmOnSprite;
		}
	}

	public void MuteSoundEffects() {
		isSFXMuted = !isSFXMuted;
		if (isSFXMuted) {
			sfxButton.GetComponent<Image> ().sprite = sfxOffSprite;
		} else {
			sfxButton.GetComponent<Image> ().sprite = sfxOnSprite;
		}
	}
}
