using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public Text levelSelectText;
	public Transform levelSelectArea;
	public GameObject levelButton;

	private int latestLevelBeat;
	private const int NUM_LEVELS = 20;

	void Start () {
		latestLevelBeat = PlayerPrefs.GetInt ("LatestLevelBeat"); //get saved variable

		//TODO: properly center level buttons; add "Levels" header
		levelSelectArea.GetComponent<RectTransform> ().localScale = new Vector3 (0.8f, 0.8f, 1);
		levelSelectArea.GetComponent<GridLayoutGroup> ().spacing = new Vector2 (Screen.width / 100, Screen.height / 100);
		levelSelectArea.GetComponent<GridLayoutGroup> ().cellSize = new Vector2 (100, 100);
		levelSelectArea.GetComponent<GridLayoutGroup> ().padding = new RectOffset (0, 0, 0, 0);

		for (int i = 1; i <= NUM_LEVELS; i++) {
			GameObject buttonCell = Instantiate (levelButton) as GameObject;
			buttonCell.name = "Level_" + i;
			buttonCell.GetComponentInChildren<Text> ().text = i.ToString();

			int tempI = i;
			buttonCell.GetComponent<Button> ().onClick.AddListener (() => { openLevel(tempI); });
			buttonCell.transform.SetParent (levelSelectArea, false);

			if (i < latestLevelBeat) {
				buttonCell.GetComponent<Button> ().enabled = false;
				//buttonCell.GetComponent<Image>().sprite = ; TODO: add sprite
			} else {
				//buttonCell.GetComponent<Image>().sprite = ; TODO: add sprite
			}

			if (latestLevelBeat == 0 && i == 1) {
				buttonCell.GetComponent<Button> ().enabled = true;
			}
		}
	}

	private void openLevel(int levelNumber) {
		SceneManager.LoadScene ("Level " + levelNumber);
	}
}
