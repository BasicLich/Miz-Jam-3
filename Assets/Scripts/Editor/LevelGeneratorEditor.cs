using UnityEngine;
using UnityEditor;

namespace MizJam.CustomEditors
{
    [CustomEditor(typeof(LevelGenerator))]
    public class LevelGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            this.DrawDefaultInspector();

            if (GUILayout.Button("Generate"))
                (this.target as LevelGenerator).Generate();
        }
    }
}