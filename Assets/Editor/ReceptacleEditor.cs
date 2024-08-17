using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Receptacle))]
public class ReceptacleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Receptacle receptacle = target as Receptacle;

        if (GUILayout.Button("Save Solution"))
        {
            receptacle.SaveSolution();
        }
    }
}
