using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;

namespace BoatGame
{
    [Overlay(typeof(SceneView), "Transform Tools")]
    public class TransformToolOverlay : ToolbarOverlay
    {
        public const string RemovePrefabId = "RemovePrefabSuffixesButton";
        public const string OrderByXId = "OrderChildrenByX";
        public const string OrderByZId = "OrderChildrenByZ";
        public const string OrderAlphaId = "OrderChildrenAlphabetically";
        public const string GridSnapId = "GridSnap";
        public const string CameraAlignId = "CameraAlign";
        
        public TransformToolOverlay() : base(RemovePrefabId, GridSnapId, OrderAlphaId, OrderByXId, OrderByZId, CameraAlignId)
        { }
    }

    [EditorToolbarElement(TransformToolOverlay.RemovePrefabId, typeof(SceneView))]
    public class RemovePrefabSuffixesButton : EditorToolbarButton
    {
        public RemovePrefabSuffixesButton()
        {
            //text = "Remove prefab suffixes";
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/EditorIcons/pencil.png");
            tooltip = "Remove prefab suffixes from selected object's children";
            clicked += OnClick;
        }

        private void OnClick()
        {
            TransformTools.RemovePrefabSuffixes();
        }
    }

    [EditorToolbarElement(TransformToolOverlay.OrderByXId, typeof(SceneView))]
    public class OrderChildrenByXButton : EditorToolbarButton
    {
        public OrderChildrenByXButton()
        {
            //text = "Order children by X";
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/EditorIcons/horizontal-flip.png");
            tooltip = "Order children on the X axis";
            clicked += OnClick;
        }

        private void OnClick()
        {
            OrderHierarchy.ReorderByXAxis();
        }
    }

    [EditorToolbarElement(TransformToolOverlay.OrderByZId, typeof(SceneView))]
    public class OrderChildrenByZButton : EditorToolbarButton
    {
        public OrderChildrenByZButton()
        {
            //text = "Order children by Z";
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/EditorIcons/vertical-flip.png");
            tooltip = "Order children on the Z axis";
            clicked += OnClick;
        }

        private void OnClick()
        {
            OrderHierarchy.ReorderByZAxis();
        }
    }

    [EditorToolbarElement(TransformToolOverlay.OrderAlphaId, typeof(SceneView))]
    public class OrderChildrenAlphabeticallyButton : EditorToolbarButton
    {
        public OrderChildrenAlphabeticallyButton()
        {
            //text = "Order children alphabetically";
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/EditorIcons/notebook.png");
            tooltip = "Order children alphabetically";
            clicked += OnClick;
        }

        private void OnClick()
        {
            OrderHierarchy.ReorderAlphabetically();
        }
    }

    [EditorToolbarElement(TransformToolOverlay.GridSnapId, typeof(SceneView))]
    public class GridSnapButton : EditorToolbarButton
    {
        public GridSnapButton()
        {
            //text = "Order children alphabetically";
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/EditorIcons/divided-square.png");
            tooltip = "Round selected to grid";
            clicked += OnClick;
        }

        private void OnClick()
        {
            RoundPositionToGrid.DoRound();
        }
    }

    [EditorToolbarElement(TransformToolOverlay.CameraAlignId, typeof(SceneView))]
    public class CameraAlignButton : EditorToolbarButton
    {
        public CameraAlignButton()
        {
            //text = "Order children alphabetically";
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/EditorIcons/photo-camera.png");
            tooltip = "Align scene view camera to game camera";
            clicked += OnClick;
        }

        private void OnClick()
        {
            var sceneCam = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlayerCamera.prefab");
            
            SceneView.lastActiveSceneView.pivot = sceneCam.transform.position;
            SceneView.lastActiveSceneView.rotation = sceneCam.transform.rotation;
        }
    }
}
