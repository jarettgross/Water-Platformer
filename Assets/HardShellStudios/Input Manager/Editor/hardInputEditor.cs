using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.IO;

[CustomEditor(typeof(hardInput))]
public class hardInputEditor : Editor
{

    hardInput myscript;
    string currentVersion = "Current Version - Alpha 1.0";

    string inputName = "";
    KeyCode keyPrime;
    KeyCode keySec;

    string[] axisOptions = new string[] { "Default Keypress", "Mouse Wheel Up", "Mouse Wheel Down", "Mouse Position X", "Mouse Position Y" };
    int axisSelected = 0;
    bool[] opened = new bool[0];

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }

    public override void OnInspectorGUI()
    {
        //GUIStyle colour1 = new GUIStyle();
        //colour1.normal.background = MakeTex(600, 1, new Color(0.4f, 0.66f, 0.34f, 0f));

        //EditorGUILayout.BeginVertical(colour1);
        // Debug
        //DrawDefaultInspector();
        //EditorGUILayout.Separator();
        //EditorGUILayout.Separator();


        //Begin

        myscript = (hardInput)target;
        bool anyOpen = false;
        for (int i = 0; i < myscript.inputs.Length; i++)
        {

            string currName = myscript.inputs[i].keyName;
            KeyCode currPrim = myscript.inputs[i].primaryKeycode;
            KeyCode currSec = myscript.inputs[i].secondaryKeycode;
            int axisType = myscript.inputs[i].axisType;
            bool[] hold = opened;
            opened = new bool[opened.Length + 1];
            for (int i2 = 0; i2 < hold.Length; i2++)
            {
                opened[i2] = hold[i2];
            }


            // Alternating Color Scheme
            Color[] colors = new Color[] { new Color(0.4f, 0.66f, 0.34f, 1f), new Color(0.35f, 0.55f, 0.65f, 1f) };
            GUIStyle style = new GUIStyle();
            style.normal.background = MakeTex(600, 1, colors[i % 2]);
            EditorGUILayout.BeginVertical(style);

            //GUIStyle styleFoldout = new GUIStyle();

            opened[i] = EditorGUILayout.Foldout(opened[i], currName);

            if (opened[i])
            {

                currName = EditorGUILayout.TextField("Name", currName);
                if (myscript.inputs[i].keyName != currName)
                    myscript.inputs[i].keyName = currName;

                currPrim = (KeyCode)EditorGUILayout.EnumPopup("Primary Input", currPrim);
                if (myscript.inputs[i].primaryKeycode != currPrim)
                    myscript.inputs[i].primaryKeycode = currPrim;

                currSec = (KeyCode)EditorGUILayout.EnumPopup("Secondary Input", currSec);
                if (myscript.inputs[i].secondaryKeycode != currSec)
                    myscript.inputs[i].secondaryKeycode = currSec;

                axisType = EditorGUILayout.Popup("Axis Option", axisType, axisOptions);
                if (myscript.inputs[i].axisType != axisType)
                    myscript.inputs[i].axisType = axisType;

                EditorGUILayout.Separator();

                if (GUILayout.Button("Delete Key"))
                    deleteSelected(i);
                anyOpen = true;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
        }

        // Alternating Color Scheme
        GUIStyle colour = new GUIStyle();
        colour.normal.background = MakeTex(600, 1, new Color(0.65f, 0.35f, 0.35f, 1));

        if (anyOpen)
        {
            if (GUILayout.Button("Hide All Keys"))
            {
                for (int i = 0; i < opened.Length; i++)
                {
                    opened[i] = false;
                }
            }
        }
        else
        {
            if (GUILayout.Button("Show All Keys"))
            {
                for (int i = 0; i < opened.Length; i++)
                {
                    opened[i] = true;
                }
            }
        }

        EditorGUILayout.BeginVertical(colour);
        // Layout  myscript.inputs For key creation
        EditorGUILayout.LabelField("Create Control");
        inputName = EditorGUILayout.TextField("Key Name", inputName);
        keyPrime = (KeyCode)EditorGUILayout.EnumPopup("Primary Input", keyPrime);
        keySec = (KeyCode)EditorGUILayout.EnumPopup("Secondary Input", keySec);
        axisSelected = EditorGUILayout.Popup("Axis Option", axisSelected, axisOptions);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        //Create Input From Options
        if (GUILayout.Button("Add Input"))
            addInput();

        //Remove Last Input
        if (GUILayout.Button("Remove Input") && myscript.inputs.Length > 0)
            removeInput();

        EditorGUILayout.Separator();

        if (GUILayout.Button("Copy Inputs"))
            UnityEditorInternal.ComponentUtility.CopyComponent(myscript);

        if (GUILayout.Button("Paste Inputs"))
            UnityEditorInternal.ComponentUtility.PasteComponentValues(myscript);

        //if (GUILayout.Button("Apply Changes"))
        //    applyChanges();

        //if (GUILayout.Button("Load Existing"))
        //    applyChanges();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(myscript);
            serializedObject.ApplyModifiedProperties();
        }

        //EditorGUILayout.EndVertical();
        EditorGUILayout.LabelField(currentVersion);
    }


    void addInput()
    {
        hardInput.givenInputs[] savedInputs = new hardInput.givenInputs[] { };
        savedInputs = myscript.inputs;
        myscript.inputs = new hardInput.givenInputs[myscript.inputs.Length + 1];

        for (int i = 0; i < savedInputs.Length; i++)
        {
            myscript.inputs[i].keyName = savedInputs[i].keyName;
            myscript.inputs[i].primaryKeycode = savedInputs[i].primaryKeycode;
            myscript.inputs[i].secondaryKeycode = savedInputs[i].secondaryKeycode;
            myscript.inputs[i].axisType = savedInputs[i].axisType;
        }

        myscript.inputs[myscript.inputs.Length - 1].keyName = inputName;
        myscript.inputs[myscript.inputs.Length - 1].axisType = axisSelected;
        myscript.inputs[myscript.inputs.Length - 1].axisType = 0;
        myscript.inputs[myscript.inputs.Length - 1].primaryKeycode = keyPrime;
        myscript.inputs[myscript.inputs.Length - 1].secondaryKeycode = keySec;

        //Reset The Selection
        inputName = "";
        axisSelected = 0;
        keyPrime = KeyCode.None;
        keySec = KeyCode.None;

    }

    void deleteSelected(int getname)
    {
        hardInput.givenInputs[] savedInputs = new hardInput.givenInputs[] { };
        savedInputs = myscript.inputs;
        myscript.inputs = new hardInput.givenInputs[myscript.inputs.Length - 1];
        int saved = 0;

        for (int i = 0; i < myscript.inputs.Length; i++)
        {
            if (saved != getname)
            {
                myscript.inputs[i].keyName = savedInputs[saved].keyName;
                myscript.inputs[i].primaryKeycode = savedInputs[saved].primaryKeycode;
                myscript.inputs[i].secondaryKeycode = savedInputs[saved].secondaryKeycode;
                myscript.inputs[i].axisType = savedInputs[saved].axisType;
            }
            else
            {
                saved++;
                myscript.inputs[i].keyName = savedInputs[saved].keyName;
                myscript.inputs[i].primaryKeycode = savedInputs[saved].primaryKeycode;
                myscript.inputs[i].secondaryKeycode = savedInputs[saved].secondaryKeycode;
                myscript.inputs[i].axisType = savedInputs[saved].axisType;
            }
            saved++;
        }

        //Reset The Selection
        inputName = "";
        axisSelected = 0;
        keyPrime = KeyCode.None;
        keySec = KeyCode.None;
    }

    void removeInput()
    {
        hardInput.givenInputs[] savedInputs = new hardInput.givenInputs[] { };
        savedInputs = myscript.inputs;
        myscript.inputs = new hardInput.givenInputs[myscript.inputs.Length - 1];

        if (savedInputs.Length - 1 > 0)
        {

            for (int i = 0; i < savedInputs.Length - 1; i++)
            {
                myscript.inputs[i].keyName = savedInputs[i].keyName;
                myscript.inputs[i].primaryKeycode = savedInputs[i].primaryKeycode;
                myscript.inputs[i].secondaryKeycode = savedInputs[i].secondaryKeycode;
                myscript.inputs[i].axisType = savedInputs[i].axisType;
            }

            //myscript.inputs[myscript.inputs.Length - 1].keyName = inputName;
            //myscript.inputs[myscript.inputs.Length - 1].axisType = axisSelected;
            //myscript.inputs[myscript.inputs.Length - 1].axisType = 0;
            //myscript.inputs[myscript.inputs.Length - 1].primaryKeycode = keyPrime;
            //myscript.inputs[myscript.inputs.Length - 1].secondaryKeycode = keySec;

            //Reset The Selection
            inputName = "";
            axisSelected = 0;
            keyPrime = KeyCode.None;
            keySec = KeyCode.None;
        }
    }

    void applyChanges()
    {
        string half1 = "";
        string half2 = "";

        System.IO.StreamReader halfFile = new System.IO.StreamReader(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)).ToString().Replace("hardInputEditor.cs", "codestart.txt"));
        half1 = halfFile.ReadToEnd();

        System.IO.StreamReader endFile = new System.IO.StreamReader(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)).ToString().Replace("hardInputEditor.cs", "codeend.txt"));
        half2 = endFile.ReadToEnd();

        //string path = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)).ToString().Replace("hardInputEditor.cs", "hInput.cs");
        string path = "Assets/hInput.cs";

        if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);

        for (int i = 0; i < myscript.inputs.Length; i++)
        {
            string input1 = "{ \"" + myscript.inputs[i].keyName + "\", new hardKey( \"" + myscript.inputs[i].keyName + "\", KeyCode." + myscript.inputs[i].primaryKeycode + ", KeyCode." + myscript.inputs[i].secondaryKeycode + ", " + myscript.inputs[i].axisType + ")}";
            //string inputFirst = ("hardKey input" + i + " = new hardKey(\"" + myscript.inputs[i].keyName + "\", KeyCode." + myscript.inputs[i].primaryKeycode + ", KeyCode." + myscript.inputs[i].secondaryKeycode + ", " + myscript.inputs[i].axisType + "); \n");
            //string inputsecond = "keyMaps.Add(input" + i + ".keyName, input" + i + ");\n";

            //Debug.Log(inputFirst);
            //Debug.Log(inputsecond);
            if (i + 1 != myscript.inputs.Length)
                input1 += ", \n";


            half1 = half1 + input1;

        }

        half1 = half1 + half2;

        System.IO.StreamWriter file = new System.IO.StreamWriter(path);
        file.WriteLine(half1);
        file.Close();
        AssetDatabase.ImportAsset(path);
        AssetDatabase.Refresh();
    }

}
