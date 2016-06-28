using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hardInputUI : MonoBehaviour
{
    hardInput hInput = null;

    public Text displayText;
    public string keyName;
    public bool wantSecond;
    [HideInInspector]
    public bool beingBound = false;

    void Start()
    {
        hInput = GameObject.FindObjectOfType<hardInput>();
    }

    public void remapKey()
    {

        beingBound = true;
        hInput.HardStartRebind(keyName, wantSecond, gameObject.GetComponent<hardInputUI>());
    }

    void OnGUI()
    {
        if (displayText != null)
        {
            if(!beingBound)
                displayText.text = hInput.GetKeyName(keyName, wantSecond);
            else
                displayText.text = "PRESS A KEY";
        }
    }

    
}
