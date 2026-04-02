using System;
using System.Numerics;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = System.Object;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace BoatGame
{
    public class TransformTools : EditorWindow
    {
        private const int Spacing = 15;
        private static readonly Type GoType = typeof(GameObject);
        
        [MenuItem("Tools/Transform Tools Window")]
        private static void CreateWindow()
        {
            var window = GetWindow<TransformTools>("Transform Tools");
            window.titleContent = new GUIContent("Transform Tools");
        }

        private bool _usePrefabTransformAsBase = true;
        private bool _useLinearScale = true;
        
        private Vector3 _positionVariance = Vector3.zero;
        private Vector3 _rotationVariance = Vector3.zero;
        private Vector3 _scaleVariance = Vector3.zero;
        private Vector2 _linearScaleRange = Vector2.zero;

        private int _angleSnap;
        private float _floorOffset = 0.01f;
        private GameObject _prefabForSwap;

        private void OnGUI()
        {
            var selection = Selection.gameObjects;

            if (selection == null || selection.Length == 0)
            {
                GUILayout.Label("No objects selected");
                return;
            }
            
            GUILayout.Label($"{selection.Length} objects selected");
            GUILayout.Space(Spacing);

            _usePrefabTransformAsBase =
                GUILayout.Toggle(_usePrefabTransformAsBase, "Use prefab transform as base");
            GUILayout.Space(Spacing);
            _positionVariance = EditorGUILayout.Vector3Field("Position variance", _positionVariance);
            GUILayout.Space(Spacing);
            _rotationVariance = EditorGUILayout.Vector3Field("Rotation variance", _rotationVariance);
            _angleSnap = EditorGUILayout.IntField("Rotation snap", _angleSnap);
            GUILayout.Space(Spacing);
            _useLinearScale = GUILayout.Toggle(_useLinearScale, "Linear scale");

            if (_useLinearScale)
                _linearScaleRange = EditorGUILayout.Vector2Field("Linear scale variance (either side of Vector3.one)", _linearScaleRange);
            else _scaleVariance = EditorGUILayout.Vector3Field("Scale variance (either side of Vector3.one)", _scaleVariance);
            GUILayout.Space(Spacing);

            if (GUILayout.Button("Randomize"))
                RandomizeScale(selection);
            
            GUILayout.Space(Spacing);
            GUILayout.Space(Spacing);

            _prefabForSwap = EditorGUILayout.ObjectField("Swap prefab", _prefabForSwap, GoType, false) as GameObject;

            if (_prefabForSwap && GUILayout.Button("Swap selected prefabs"))
                SwapPrefabs(selection);
            
            GUILayout.Space(Spacing);

            _floorOffset = EditorGUILayout.FloatField("Snap floor offset", _floorOffset);
            
            if (GUILayout.Button("Snap to floor"))
            {
                SnapToFloor();
            }
        }

        private void RandomizeScale(GameObject[] selection)
        {
            for (int i = 0, iMax = selection.Length; i < iMax; i++)
            {
                var t = selection[i].transform;
                Undo.RecordObject(t, "Randomized selection");

                Transform prefab = null;

                if (_usePrefabTransformAsBase)
                {
                    var origin = PrefabUtility.GetCorrespondingObjectFromOriginalSource(selection[i]);

                    if (origin)
                    {
                        prefab = origin.transform;
                    }
                }

                var usePrefab = _usePrefabTransformAsBase && prefab;

                t.position += new Vector3(
                    Random.Range(-_positionVariance.x, _positionVariance.x),
                    Random.Range(-_positionVariance.y, _positionVariance.y),
                    Random.Range(-_positionVariance.z, _positionVariance.z));

                var randomRotation = Quaternion.Euler(
                    SnapAngle(Random.Range(-_rotationVariance.x, _rotationVariance.x)),
                    SnapAngle(Random.Range(-_rotationVariance.y, _rotationVariance.y)),
                    SnapAngle(Random.Range(-_rotationVariance.z, _rotationVariance.z)));

                var randomScale = Vector3.zero;
                
                if (_useLinearScale)
                {
                    var r = Random.Range(_linearScaleRange.x, _linearScaleRange.y);
                    randomScale.Set(r, r, r);
                }
                else
                {
                    randomScale = new Vector3(
                        Random.Range(-_scaleVariance.x, _scaleVariance.x),
                        Random.Range(-_scaleVariance.y, _scaleVariance.y),
                        Random.Range(-_scaleVariance.z, _scaleVariance.z));
                }
                
                if (usePrefab)
                {
                    t.rotation = prefab.rotation * randomRotation;
                    t.localScale = prefab.localScale + randomScale;
                }
                else
                {
                    t.rotation *= randomRotation;
                    t.localScale += randomScale;
                }
            }
        }

        private float SnapAngle(float angle)
        {
            if (_angleSnap == 0)
                return angle;

            return Mathf.Round(angle / _angleSnap) * _angleSnap;
        }

        private void SwapPrefabs(GameObject[] selection)
        {
            for (int i = 0, iMax = selection.Length; i < iMax; i++)
            {
                var toBeSwapped = selection[i];
                var t = toBeSwapped.transform;
                var childIndex = t.GetSiblingIndex();

                var newInstance = PrefabUtility.InstantiatePrefab(_prefabForSwap, t.parent) as GameObject;
                newInstance.transform.SetPositionAndRotation(t.position, t.rotation);
                newInstance.transform.localScale = t.localScale;
                newInstance.transform.SetSiblingIndex(childIndex);
                Undo.RegisterCreatedObjectUndo(newInstance, "Prefab Swap");
                Undo.DestroyObjectImmediate(toBeSwapped);
            }
        }

        private void SnapToFloor()
        {
            var allSelected = Selection.gameObjects;

            for (int i = 0, iMax = allSelected.Length; i < iMax; i++)
            {
                var go = allSelected[i];
                var t = go.transform;
                var pos = t.position;

                var hits = Physics.RaycastAll(pos + Vector3.up, Vector3.down * 10f);

                var bestDistance = float.MaxValue;
                var bestIndex = -1;
                
                for (int j = 0; j < hits.Length; j++)
                {
                    var hit = hits[j];

                    if (hit.transform.gameObject == go)
                        continue;

                    if (hit.distance < bestDistance)
                    {
                        bestDistance = hit.distance;
                        bestIndex = j;
                    }
                }

                if (bestIndex > -1)
                {
                    Undo.RecordObject(t, "Snap to floor");
                    t.position = hits[bestIndex].point + Vector3.up * _floorOffset;
                }
            }
        }

        [MenuItem("Tools/Remove prefab suffixes")]
        public static void RemovePrefabSuffixes()
        {
            if (Selection.activeGameObject == null)
                return;

            var selection = Selection.activeGameObject;
            var t = selection.transform;
            var childCount = t.childCount;

            var regex = new Regex("\\([0-9]*\\)");

            for (int i = 0, iMax = childCount; i < iMax; i++)
            {
                var c = t.GetChild(i);
                var newName = regex.Replace(c.name, string.Empty).Trim();
                c.name = newName;
            }
        }
    }
}
