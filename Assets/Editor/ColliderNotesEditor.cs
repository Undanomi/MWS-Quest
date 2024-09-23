using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(ColliderNotes))]
    public class ColliderNotesEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ColliderNotes colliderManager = (ColliderNotes)target;

            // デフォルトのインスペクタを表示
            DrawDefaultInspector();

            // 見やすい形でメモを表示
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Collider Roles", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox("Circle Collider:" + colliderManager.circleColliderNote, MessageType.Info);
            EditorGUILayout.HelpBox("Capsule Collider: " + colliderManager.capsuleColliderNote, MessageType.Info);
        }
    }
}
