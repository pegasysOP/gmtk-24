using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Receptacle))]
public class ReceptacleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Receptacle receptacle = target as Receptacle;

        if (GUILayout.Button("Add Solution"))
        {
            receptacle.AddSolution();
            EditorUtility.SetDirty(receptacle);
        }

        if (GUILayout.Button("Clear Solutions"))
        {
            receptacle.ClearSolutions();
            EditorUtility.SetDirty(receptacle);
        }
    }
}
