using UnityEngine;
using System.Collections;

public class IceBlock : MonoBehaviour {

	public float growSpeed;
	public float shrinkSpeed;
	public float maxIceWidth;
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.localScale.y > 0) {
			ShrinkIceBlock ();
		} else {
			Vector3 tempScale = gameObject.transform.localScale;
			tempScale.y = 0.0f;
			gameObject.transform.localScale = tempScale;
		}
	}

	//Shrink the ice block in y whenever it has some width
	private void ShrinkIceBlock() {
		Vector3 tempScale = gameObject.transform.localScale;
		tempScale.y -= Time.fixedDeltaTime / shrinkSpeed;
		gameObject.transform.localScale = tempScale;
	}
}
