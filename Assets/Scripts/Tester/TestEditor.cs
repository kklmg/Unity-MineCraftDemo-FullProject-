using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Assets.Scripts.Tester
{
    [CustomEditor(typeof(TestScript))]
    class TestEditor : Editor
    {
        private TestScript testTarget;

        public void OnEnable()
        {
            testTarget = target as TestScript;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("ReMake Texture", GUILayout.Width(200)))
            {
                testTarget.MakeTexture();
            }

            GUILayout.Space(20);
            
            serializedObject.ApplyModifiedProperties();
        }
        private void OnDisable()
        {
           // EditorUtility.SetDirty(target);
        }
    }
}
#endif