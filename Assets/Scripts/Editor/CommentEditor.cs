using UnityEditor;
using UnityEngine;

namespace BoatGame
{
    [CustomEditor(typeof(Comment))]
    public class CommentEditor : Editor
    {
        private static readonly GUILayoutOption HeightOption = GUILayout.Height(100);
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var prop = serializedObject.FindProperty("comment");
            var s = GUILayout.TextArea(prop.stringValue, int.MaxValue, HeightOption);
            prop.stringValue = s;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
