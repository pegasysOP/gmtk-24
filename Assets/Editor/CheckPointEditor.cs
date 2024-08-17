using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CheckPoint))]
public class CheckPointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CheckPoint checkPoint = target as CheckPoint;
        if (GUILayout.Button("Complete")) {
            checkPoint.Complete();
        }
    }
}
