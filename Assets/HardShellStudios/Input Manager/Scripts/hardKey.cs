using UnityEngine;
using System.Collections;
using System;

public class hardKey : IComparable<hardKey> {

    public string keyName;
    public KeyCode keyInput;
    public KeyCode keyInput2;
    public int keyWheelState;
    public float keyValue;

    public hardKey(string keyNameGIVE, KeyCode inputKeyGIVE, KeyCode inputKey2GIVE, int keyWheelStateGIVE)
    {
        keyName = keyNameGIVE;
        keyInput = inputKeyGIVE;
        keyInput2 = inputKey2GIVE;
        keyWheelState = keyWheelStateGIVE;
        keyValue = 0;
    }

    public int CompareTo(hardKey other)
    {
        return 1;
    }
}
