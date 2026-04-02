using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BoatGame
{
    public static class OrderHierarchy
    {
        [MenuItem("Tools/Order Hierarchy/Alphabetically")]
        public static void ReorderAlphabetically()
        {
            if (!GetChildrenOfSelected(out var allChildren, out var childCount))
                return;
            
            allChildren.Sort((t1, t2) => String.Compare(t1.name, t2.name, StringComparison.Ordinal));

            for (int i = 0, iMax = childCount; i < iMax; i++)
                allChildren[i].SetSiblingIndex(i);

            GC.Collect();
        }

        [MenuItem("Tools/Order Hierarchy/X Axis")]
        public static void ReorderByXAxis()
        {
            if (!GetChildrenOfSelected(out var allChildren, out var childCount))
                return;

            allChildren.Sort((t1, t2) =>
            {
                var xComp = t1.position.x.CompareTo(t2.position.x);

                if (xComp == 0)
                    return t1.position.z.CompareTo(t2.position.z);

                return xComp;
            });

            for (int i = 0, iMax = childCount; i < iMax; i++)
                allChildren[i].SetSiblingIndex(i);

            GC.Collect();
        }

        [MenuItem("Tools/Order Hierarchy/Z Axis")]
        public static void ReorderByZAxis()
        {
            if (!GetChildrenOfSelected(out var allChildren, out var childCount))
                return;

            allChildren.Sort((t1, t2) =>
            {
                var zComp = t1.position.z.CompareTo(t2.position.z);

                if (zComp == 0)
                    return t1.position.x.CompareTo(t2.position.x);

                return zComp;
            });

            for (int i = 0, iMax = childCount; i < iMax; i++)
                allChildren[i].SetSiblingIndex(i);

            GC.Collect();
        }

        private static bool GetChildrenOfSelected(out List<Transform> children, out int childCount)
        {
            var selection = Selection.transforms;

            if (selection.Length > 1 || selection.Length == 0)
            {
                EditorUtility.DisplayDialog("Error", "Select a single transform.", "Ok");
                childCount = 0;
                children = null;
                return false;
            }

            childCount = selection[0].childCount;
            children = new List<Transform>(childCount);

            for (int i = 0, iMax = childCount; i < iMax; i++)
            {
                children.Add(selection[0].GetChild(i));
            }

            return true;
        }
    }
}
