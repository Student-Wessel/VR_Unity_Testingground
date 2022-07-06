using System;
using UnityEditor;
using UnityEngine;

namespace Temps
{
    [CustomEditor(typeof(ApplyMaterial))]
    public class ApplyMaterialEditor : Editor
    {
        private ApplyMaterial applyMaterials;

        private void OnEnable()
        {
            applyMaterials = (ApplyMaterial) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Apply Material"))
            {
                applyMaterials.Apply(applyMaterials.transform);
            }
        }
    }
}
