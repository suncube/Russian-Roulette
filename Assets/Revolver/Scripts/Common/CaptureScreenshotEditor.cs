#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (CaptureScreenshot))]
public class CaptureScreenshotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var myScript = (CaptureScreenshot)target;
        if (GUILayout.Button("Take Screenshot"))
        {
            myScript.TakeScreenshot();
        }
    }
}
#endif