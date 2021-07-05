using UnityEngine;
using UnityEditor;

using Assets.Scripts.NWorld;
using Assets.Scripts.NMesh;

#if UNITY_EDITOR
namespace Assets.Scripts.NEditor
{
    [System.Serializable]
    [CustomEditor(typeof(Block))]
    class BlockEditor : Editor
    {
        private Block m_BlockInfo;

        static public Object UpMesh;
        static public Object DownMesh;
        static public Object LeftMesh;
        static public Object RightMesh;
        static public Object FrontMesh;
        static public Object BackMesh;


        public void OnEnable()
        {
            m_BlockInfo = target as Block;
        }
        public override void OnInspectorGUI()
        {

            UpMesh = EditorGUILayout.ObjectField(new GUIContent("Up Mesh"), UpMesh, typeof(DynamicMeshScObj), true);
            DownMesh = EditorGUILayout.ObjectField(new GUIContent("Down Mesh"), DownMesh, typeof(DynamicMeshScObj), true);
            LeftMesh = EditorGUILayout.ObjectField(new GUIContent("Left Mesh"), LeftMesh, typeof(DynamicMeshScObj), true);
            RightMesh = EditorGUILayout.ObjectField(new GUIContent("Right Mesh"), RightMesh, typeof(DynamicMeshScObj), true);
            FrontMesh = EditorGUILayout.ObjectField(new GUIContent("Front Mesh"), FrontMesh, typeof(DynamicMeshScObj), true);
            BackMesh = EditorGUILayout.ObjectField(new GUIContent("Back Mesh"), BackMesh, typeof(DynamicMeshScObj), true);

            if (GUILayout.Button("Assign Mesh: Cube 1x1x1", GUILayout.Width(200)))
            {
                m_BlockInfo.LeftTile.OriginMesh = (DynamicMeshScObj)LeftMesh;
                m_BlockInfo.RightTile.OriginMesh = (DynamicMeshScObj)RightMesh;
                m_BlockInfo.UpTile.OriginMesh = (DynamicMeshScObj)UpMesh;
                m_BlockInfo.DownTile.OriginMesh = (DynamicMeshScObj)DownMesh;
                m_BlockInfo.FrontTile.OriginMesh = (DynamicMeshScObj)FrontMesh;
                m_BlockInfo.BackTile.OriginMesh = (DynamicMeshScObj)BackMesh;
            }

            GUILayout.Space(20);

            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();            
        }
        private void OnDisable()
        {
            EditorUtility.SetDirty(target);
        }

    }
}
#endif
