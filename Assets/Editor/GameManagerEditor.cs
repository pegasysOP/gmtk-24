using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10f);
        GUILayout.Label("Controls", EditorStyles.boldLabel);
        GUILayout.Space(5f);

        if (GUILayout.Button("Start Game"))
        {
            GameManager.Instance.StartGame?.Invoke();
        }
    }
}
