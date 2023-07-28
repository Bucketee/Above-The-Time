using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(GameEvent), editorForChildClasses: true)]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        GameEvent gameEvent = (GameEvent)target;

        GUILayout.Space(30);

        GUI.enabled = Application.isPlaying;
        if (GUILayout.Button("Raise"))
        {
            gameEvent.Raise(null, null);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif