using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Manager : MonoBehaviour
{
    hardInput hInput = null;
    public GameObject menu;

    // Use this for initialization
    void Start ()
    {
        hInput = GameObject.FindObjectOfType<hardInput>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (hInput.GetKeyDown("Menu"))
            if (menu.activeSelf)
            {
                menu.SetActive(false);
                hInput.MouseLock(true);
                hInput.MouseVisble(false);
            }
            else
            {
                menu.SetActive(true);
                hInput.MouseLock(false);
                hInput.MouseVisble(true);
            }
	}
}
