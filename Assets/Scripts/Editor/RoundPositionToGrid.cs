using UnityEditor;
using UnityEngine;

namespace BoatGame
{
    public static class RoundPositionToGrid
    {
        [MenuItem("Tools/Round Position to Grid")]
        public static void DoRound()
        {
            var selectedObjects = Selection.gameObjects;

            if (selectedObjects == null || selectedObjects.Length == 0)
                return;
            
            var snap = EditorSnapSettings.move;
            var halfSnap = snap / 2f;

            for (int i = 0, iMax = selectedObjects.Length; i < iMax; i++)
            {
                var selectedObject = selectedObjects[i];
                
                var t = selectedObject.transform;
                var pos = t.position;
                pos.Set(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));

                var xDiff = pos.x % snap.x;
                var yDiff = pos.y % snap.y;
                var zDiff = pos.z % snap.z;
                
                pos.Set(xDiff > halfSnap.x ? pos.x - pos.x -xDiff : pos.x + xDiff,
                    yDiff > halfSnap.y ? pos.y - pos.y -yDiff : pos.y + yDiff,
                    zDiff > halfSnap.z ? pos.z - pos.z -zDiff : pos.z + zDiff);
                
                Undo.RecordObject(selectedObjects[i], "Round to grid");
                t.position = pos;
            }
        }
    }
}
