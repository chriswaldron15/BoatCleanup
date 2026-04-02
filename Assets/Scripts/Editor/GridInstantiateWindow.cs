using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BoatGame
{
    public class GridInstantiateWindow : EditorWindow
    {
        private const int Spacing = 5;
        private static readonly Type GoType = typeof(GameObject);

        [MenuItem("Tools/Grid Instantiate")]
        private static void CreateWindow()
        {
            var window = GetWindow<GridInstantiateWindow>("Grid Instantiate");
            window.titleContent = new GUIContent("Grid Instantiate");
        }

        private GameObject[] _prefabs = new GameObject[1];
        private GameObject Prefab => _prefabs == null || _prefabs.Length == 0 ? null : _prefabs[0];
        private Vector3Int _gridSize = Vector3Int.one;
        private Vector3 _spacing = Vector3.one;
        private Vector3 _startPosition;
        private bool _useAssetSizeForSpacing = true;

        private int _count = 1;
        private Vector3 _objectSize;
        private Vector3 _objectPositionOffset;
        private bool _drawHandles;
        private int _lastSelectedIndex = -1;

        private void OnEnable()
        {
            SceneView.duringSceneGui -= DrawSceneGUI;
            SceneView.duringSceneGui += DrawSceneGUI;
        }

        private void OnFocus()
        {
            _drawHandles = true;
        }

        private void OnLostFocus()
        {
            _drawHandles = false;
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            _count = EditorGUILayout.IntField("Prefab count", _count);

            if (EditorGUI.EndChangeCheck())
            {
                _count = Mathf.Clamp(_count, 0, 12);
                Array.Resize(ref _prefabs, _count);
            }

            for (int i = 0, iMax = _count; i < iMax; i++)
            {
                _prefabs[i] = (GameObject)EditorGUILayout.ObjectField($"Prefab {i + 1}", _prefabs[i], GoType, false);
            }

            if (Prefab == null)
                return;

            if (EditorGUI.EndChangeCheck())
                UpdatePrefabSpacingAndBounds();

            _gridSize = EditorGUILayout.Vector3IntField("Grid size/ count per axis", _gridSize);
            _spacing = EditorGUILayout.Vector3Field("Spacing", _spacing);
            _startPosition = EditorGUILayout.Vector3Field("Start position", _startPosition);
            Handles.DrawWireCube(_startPosition, Vector3.one * 5f);
            _useAssetSizeForSpacing = GUILayout.Toggle(_useAssetSizeForSpacing, "Use asset size/ bounds for spacing");

            GUILayout.Space(10);

            if (GUILayout.Button("Create grid"))
                CreateOrShowInstantiations(false);
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Undo"))
                Undo.PerformUndo();
        }

        private void CreateOrShowInstantiations(bool previewOnly)
        {   
            Transform parent = null;

            if (!previewOnly)
            {
                parent = new GameObject($"GridInstance_{DateTime.Now.Ticks}").transform;
                Undo.RegisterCreatedObjectUndo(parent.gameObject, "GridInstantiate");
                _lastSelectedIndex = -1;
            }
            
            var rot = Prefab.transform.rotation;
            var actualSpacing = new Vector3(_objectSize.x * _spacing.x, _objectSize.y * _spacing.y, _objectSize.z * _spacing.z);

            for (int x = 0; x < _gridSize.x; x++)
            {
                float xOffset = x * actualSpacing.x + _startPosition.x;
                    
                for (int y = 0; y < _gridSize.y; y++)
                {
                    float yOffset = y * actualSpacing.y + _startPosition.y;
                        
                    for (int z = 0; z < _gridSize.z; z++)
                    {
                        float zOffset = z * actualSpacing.z + _startPosition.z;
                        var pos = new Vector3(xOffset, yOffset, zOffset);

                        if (previewOnly)
                        {
                            Handles.DrawWireCube(pos + _objectPositionOffset, _objectSize);
                        }
                        else
                        {
                            int newIndex;

                            do
                            {
                                newIndex = Random.Range(0, _count);
                            } while (_prefabs.Length > 1 && newIndex == _lastSelectedIndex);

                            _lastSelectedIndex = newIndex;
                            var randPrefab = _prefabs[_lastSelectedIndex];

                            if (randPrefab == null)
                            {
                                Debug.LogError($"Null prefab in array at index {_lastSelectedIndex}");
                                continue;
                            }
                            
                            var instance = PrefabUtility.InstantiatePrefab(randPrefab, parent) as GameObject;
                            instance.transform.SetPositionAndRotation(pos, rot);
                            Undo.RegisterCreatedObjectUndo(instance, "GridInstantiate");
                        }
                    }
                }
            }
        }

        private void DrawSceneGUI(SceneView obj)
        {
            if (!_drawHandles)
                return;

            _startPosition = Handles.PositionHandle(_startPosition, Quaternion.identity);
            
            if (Prefab)
                CreateOrShowInstantiations(true);
        }

        private void UpdatePrefabSpacingAndBounds()
        {
            if (Prefab == null)
                return;

            var renderers = Prefab.GetComponentsInChildren<Renderer>();
            Bounds? bounds = null;

            foreach (var r in renderers)
            {
                if (bounds == null)
                    bounds = r.bounds;
                bounds.Value.Encapsulate(r.bounds);
            }

            if (bounds.HasValue)
            {
                _objectSize = bounds.Value.size;
                _objectPositionOffset = bounds.Value.center;
                return;
            }
            
            Debug.LogError("Could not determine bounds from prefab - it might not have renderers");

            _objectSize = Vector3.one;
            _objectPositionOffset = Vector3.zero;
        }
    }
}
