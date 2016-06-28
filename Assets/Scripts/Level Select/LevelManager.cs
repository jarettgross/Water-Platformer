using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public Text levelSelectText;
	public Transform levelSelectArea;
	public GameObject levelButton;
	public Sprite lockedLevelSprite;
	public Sprite unlockedLevelSprite;

	private int latestLevelBeat;
	private const int NUM_LEVELS = 20;

	private float widthScale = 0.8f;
	private float heightScale = 0.8f;

	public GameObject bas;
	public Font buttonFont;

	public GameObject controlsPanel;
	public Text left, right, jump, shoot_left, shoot_right, shoot_up;

	void Start () {
		latestLevelBeat = PlayerPrefs.GetInt ("LatestLevelBeat"); //get saved variable

		levelSelectText.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height * (1 - heightScale));
		levelSelectText.GetComponent<RectTransform> ().localPosition = new Vector3 (0, levelSelectText.transform.parent.position.y * heightScale, 0);

		levelSelectArea.GetComponent<RectTransform> ().localScale = new Vector3 (widthScale, heightScale, 1);
		levelSelectArea.GetComponent<GridLayoutGroup> ().spacing = new Vector2 (widthScale * Screen.width / 16, heightScale * Screen.height / 15);
		levelSelectArea.GetComponent<GridLayoutGroup> ().cellSize = new Vector2 (widthScale * Screen.width / 5, heightScale * Screen.height / 4);
		levelSelectArea.GetComponent<GridLayoutGroup> ().padding = new RectOffset (0, 0, Mathf.FloorToInt(Screen.height * 0.1f), 0);

		for (int i = 1; i <= NUM_LEVELS; i++) {
			GameObject buttonCell = Instantiate (levelButton) as GameObject;
			buttonCell.name = "Level_" + i;
			buttonCell.GetComponentInChildren<Text> ().color = new Color (0, 0, 0);
			buttonCell.GetComponentInChildren<Text> ().font = buttonFont;
			buttonCell.GetComponentInChildren<Text> ().text = i.ToString();

			int tempI = i;
			buttonCell.GetComponent<Button> ().onClick.AddListener (() => { openLevel(tempI); });
			buttonCell.transform.SetParent (levelSelectArea, false);

			if (i <= (latestLevelBeat + 1)) {
				buttonCell.GetComponent<Image> ().sprite = unlockedLevelSprite;
			} else {
				buttonCell.GetComponent<Button> ().enabled = false;
				buttonCell.GetComponent<Image> ().sprite = lockedLevelSprite;
			}

			if (latestLevelBeat == 0 && i == 1) {
				buttonCell.GetComponent<Button> ().enabled = true;
				buttonCell.GetComponent<Image> ().sprite = unlockedLevelSprite;
			}
		}
	}

	public void openLevel(int levelNumber) {
		if (bas.GetComponent<ButtonAndSoundManager>().isBGMMuted) {
			PlayerPrefs.SetInt ("BGMMute", 1);	
		} else {
			PlayerPrefs.SetInt ("BGMMute", 0);
		}
		PlayerPrefs.Save();
		SceneManager.LoadScene ("Level " + levelNumber);
	}

	public void openControls() {
		controlsPanel.SetActive (true);
	}

	public void saveClose() {
		controlsPanel.SetActive (false);
	}
}
