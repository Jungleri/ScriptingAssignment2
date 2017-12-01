using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CityGenerator))]
public class GenerateCityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CityGenerator cityGenScript = (CityGenerator)target;
        if (GUILayout.Button("Build City"))
        {
            cityGenScript.GenerateCity();
        }

        if (cityGenScript.autoUpdate)
        {
            cityGenScript.GenerateCity();
        }
    }
}