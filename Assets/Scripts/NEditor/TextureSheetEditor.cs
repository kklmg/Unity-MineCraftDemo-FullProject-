using UnityEditor;
using UnityEngine;


using Assets.Scripts.NWorld;

#if UNITY_EDITOR
namespace Assets.Scripts.NEditor
{
    [CustomEditor(typeof(TextureSheet))]
    class TextureSheetEditor : Editor
    {
        TextureSheet m_TextureInfo;
        int m_nRow;
        int m_nColumn;
        public void OnEnable()
        {
            m_TextureInfo = target as TextureSheet;
            m_nRow = m_TextureInfo.MaxRow;
            m_nColumn = m_TextureInfo.MaxColumn;
        }

        public override void OnInspectorGUI()
        {
            m_nRow = EditorGUILayout.IntSlider(m_nRow, 0, 64);
            m_nColumn = EditorGUILayout.IntSlider(m_nColumn, 0, 64);

            if (GUILayout.Button("setRow", GUILayout.Width(200)))
            {
                m_TextureInfo.SetMaxRow(m_nRow);
            }
            if (GUILayout.Button("setColumn", GUILayout.Width(200)))
            {
                m_TextureInfo.SetMaxColumn(m_nColumn);
            }

            GUILayout.Space(20);

            base.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }

        private void OnDisable()
        {
            //EditorUtility.SetDirty(target);
        }

    }
}
#endif