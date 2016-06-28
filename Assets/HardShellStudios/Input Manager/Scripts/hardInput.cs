using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[System.Serializable]
public class hardInput : MonoBehaviour {

    [SerializeField]
    public givenInputs[] inputs = new givenInputs[] {  };
    Dictionary<string, hardKey> keyMaps = new Dictionary<string, hardKey>();
    string currentRebind = "";
    bool replaceSecond = false;
    hardInputUI currentBindFrom;

    // Use this for initialization
    void Start()
    {
		DontDestroyOnLoad (gameObject);
        for (int i = 0; i < inputs.Length; i++)
        {
            int axisCode = inputs[i].axisType;

            if (inputs[i].axisType > 0)
            {
                inputs[i].primaryKeycode = KeyCode.None;
                inputs[i].secondaryKeycode = KeyCode.None;
            }

            hardKey addInput = new hardKey(inputs[i].keyName, inputs[i].primaryKeycode, inputs[i].secondaryKeycode, axisCode);
            keyMaps.Add(addInput.keyName, addInput);
        }

        loadBindings();
    }

    [Serializable]
    public struct givenInputs
    {
        public string keyName;
        public KeyCode primaryKeycode;
        public KeyCode secondaryKeycode;
        public int axisType;
    }

    // Basic Functions
    public bool GetKeyDown(string keyName)
    {
        if (keyMaps[keyName].keyWheelState == 0)
        {
            if (Input.GetKeyDown(keyMaps[keyName].keyInput))
                return true;
            else if (Input.GetKeyDown(keyMaps[keyName].keyInput2))
                return true;
            else
                return false;
        }
        else
        {
            if (keyMaps[keyName].keyWheelState == 1)
                if (Input.mouseScrollDelta.y > 0)
                    return true;
                else
                    return false;
            else if (keyMaps[keyName].keyWheelState == 2)
                if (Input.mouseScrollDelta.y < 0)
                    return true;
                else
                    return false;
            else
                return false;
        }
    }

    public bool GetKey(string keyName)
    {
        if (keyMaps[keyName].keyWheelState == 0)
        {
            if (Input.GetKey(keyMaps[keyName].keyInput))
                return true;
            else if (Input.GetKey(keyMaps[keyName].keyInput2))
                return true;
            else
                return false;
        }
        else
        {
            if (keyMaps[keyName].keyWheelState == 1)
                if (Input.mouseScrollDelta.y > 0)
                    return true;
                else
                    return false;
            else if (keyMaps[keyName].keyWheelState == 2)
                if (Input.mouseScrollDelta.y < 0)
                    return true;
                else
                    return false;
            else
                return false;
        }
    }

    void OnGUI()
    {
        //currentMousePos = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    }

    public float GetAxis(string keyName, string keyName2, float gravity)
    {
        if (keyMaps[keyName].keyWheelState != 3 && keyMaps[keyName].keyWheelState != 4)
        {
            if (GetKey(keyName))
                keyMaps[keyName].keyValue += gravity * Time.deltaTime;
            if (GetKey(keyName2))
                keyMaps[keyName].keyValue -= gravity * Time.deltaTime;

            if (!GetKey(keyName) && !GetKey(keyName2))
                keyMaps[keyName].keyValue = Mathf.Lerp(keyMaps[keyName].keyValue, 0, gravity * Time.deltaTime);

            keyMaps[keyName].keyValue = Mathf.Clamp(keyMaps[keyName].keyValue, -1, 1);
        }
        else if (keyMaps[keyName].keyWheelState == 3)
        {
            float xMovement = Input.GetAxisRaw("Mouse X") * gravity;
            keyMaps[keyName].keyValue = xMovement;
        }
        else if (keyMaps[keyName].keyWheelState == 4)
        {
            float yMovement = Input.GetAxisRaw("Mouse Y") * gravity;
            keyMaps[keyName].keyValue = yMovement;
        }

        return keyMaps[keyName].keyValue;
    }

    public string GetKeyName(string keyName, bool wantSecond)
    {
        string keyString = "";

        if (!wantSecond)
            keyString = keyMaps[keyName].keyInput.ToString();
        else
            keyString = keyMaps[keyName].keyInput2.ToString();

        if (keyMaps[keyName].keyWheelState == 0)
        {
            if (keyString.Contains("Alpha"))
                keyString = keyString.Replace("Alpha", "");
            else if (keyString.Contains("Keypad"))
                keyString = keyString.Replace("Keypad", "Keypad ");
            else if (keyString.Contains("Left"))
                keyString = keyString.Replace("Left", "Left ");
            else if (keyString.Contains("Right"))
                keyString = keyString.Replace("Right", "Right ");
            else if (keyString.Contains("Mouse0"))
                keyString = "Left Click";
            else if (keyString.Contains("Mouse1"))
                keyString = "Right Click";
            else if (keyString.Contains("Mouse"))
                keyString = "Mouse " + keyString.Replace("Mouse", "");
        }
        else
        {
            if (keyMaps[keyName].keyWheelState == 1)
                keyString = "Mouse Wheel Up";
            else if (keyMaps[keyName].keyWheelState == 2)
                keyString = "Mouse Wheel Down";
            else if (keyMaps[keyName].keyWheelState == 3)
                keyString = "Mouse X";
            else if (keyMaps[keyName].keyWheelState == 4)
                keyString = "Mouse Y";
        }

        return keyString;
    }

    // Various mouse operations
    public void MouseLock(bool lockedOrNot)
    {
        if (lockedOrNot)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

    public void MouseVisble(bool visibleOrNot)
    {
        Cursor.visible = visibleOrNot;
    }

    // Format strings with the returned key code
    public string FormatString(string inputText)
    {
        if (inputText.Contains("Ø"))
        {
            string[] formatString = inputText.Split('Ø');
            inputText = inputText.Replace("Ø" + formatString[1] + "Ø", GetKeyName(formatString[1], false));
            return inputText;
        }
        else
            return inputText;
    }

    // Load and save bindings
    public void loadBindings()
    {
        // Load Primary Keys
        for (var e = keyMaps.GetEnumerator(); e.MoveNext();)
        {
            if (PlayerPrefs.HasKey("settings_bindings_" + e.Current.Value.keyName))
            {
                string[] parsed = PlayerPrefs.GetString("settings_bindings_" + e.Current.Value.keyName).Split('^');
                keyMaps[e.Current.Value.keyName].keyInput = (KeyCode)System.Enum.Parse(typeof(KeyCode), parsed[0]);

                if (parsed.Length >= 2)
                    keyMaps[e.Current.Value.keyName].keyWheelState = int.Parse(parsed[1]);
                else
                    keyMaps[e.Current.Value.keyName].keyWheelState = 0;

            }
            else
            {
                print("Not Found: settings_bindings_" + e.Current.Value.keyName);
            }
        }

        // Load Secondary Keys
        for (var e = keyMaps.GetEnumerator(); e.MoveNext();)
        {
            if (PlayerPrefs.HasKey("settings_bindings_sec_" + e.Current.Value.keyName))
            {
                if (PlayerPrefs.GetString("settings_bindings_sec_" + e.Current.Value.keyName) != "None")
                    keyMaps[e.Current.Value.keyName].keyInput2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("settings_bindings_sec_" + e.Current.Value.keyName));
            }
            else
            {
                print("Not Found: settings_bindings_sec_" + e.Current.Value.keyName);
            }
        }

        SaveBindings();
    }

    public void SaveBindings()
    {
        // Save primary keys
        for (var e = keyMaps.GetEnumerator(); e.MoveNext();)
        {
            PlayerPrefs.SetString("settings_bindings_" + e.Current.Value.keyName, e.Current.Value.keyInput.ToString() + "^" + e.Current.Value.keyWheelState.ToString());
        }

        // Save secondary keys
        for (var e = keyMaps.GetEnumerator(); e.MoveNext();)
        {
            PlayerPrefs.SetString("settings_bindings_sec_" + e.Current.Value.keyName, e.Current.Value.keyInput2.ToString());
        }

        PlayerPrefs.Save();
    }

    // Rebinding Keys
    public void HardStartRebind(string keyNameGET, bool wantSecond, hardInputUI inputFrom)
    {
        //print("Rebinding: " + keyNameGET);
        currentBindFrom = inputFrom;
        replaceSecond = wantSecond;
        currentRebind = keyNameGET;
        StartCoroutine(waitForKeyPress());
    }

    IEnumerator waitForKeyPress()
    {
        yield return new WaitForEndOfFrame();

        while (!Input.anyKeyDown && Input.mouseScrollDelta.y == 0)
        {
            yield return null;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            print(Input.mouseScrollDelta.ToString());
            replaceSecond = false;
            if (Input.mouseScrollDelta.y > 0)
            {
                hardRebind(currentRebind, KeyCode.None, 1);
            }
            else
            {
                hardRebind(currentRebind, KeyCode.None, 2);
            }
        }
        else
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    hardRebind(currentRebind, kcode, 0);
                }
            }
        }
    }

    void hardRebind(string rebindName, KeyCode inputKey, int keyWheelState)
    {
        //print("Rebinding: " + rebindName + " from " + keyMaps[rebindName].keyInput.ToString() + " to " + inputKey.ToString() + " WHEELSTATE: " + keyWheelState);

        if (inputKey == KeyCode.Delete)
            inputKey = KeyCode.None;

        if (!replaceSecond)
            keyMaps[rebindName].keyInput = inputKey;
        else
            keyMaps[rebindName].keyInput2 = inputKey;
        keyMaps[rebindName].keyWheelState = keyWheelState;



        currentBindFrom.beingBound = false;
        SaveBindings();
    }
}
