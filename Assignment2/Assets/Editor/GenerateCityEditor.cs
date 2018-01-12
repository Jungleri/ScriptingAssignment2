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
        {   //When the button is pressed, generate a city block.
            cityGenScript.GenerateCity();
        }

        if (cityGenScript.autoUpdate)
        {   //If the Auto Update button is ticked, update the city ever frame (not reccomended).
            cityGenScript.GenerateCity();
        }
    }
}