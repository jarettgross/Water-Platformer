using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour {

	public GameObject creditsButton;
	public GameObject creditsInfo;

	public void Play() {
		SceneManager.LoadScene ("Level Select");
	}

	public void Credits() {
		creditsButton.SetActive (false);
		creditsInfo.SetActive (true);
	}
}
