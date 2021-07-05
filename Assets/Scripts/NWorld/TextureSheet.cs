using System;

using UnityEngine;

namespace Assets.Scripts.NWorld
{
    [Serializable]
    public class Frame
    {
        public Frame()
        {
        }

        public Frame(float l, float r, float t, float b)
        {
            left = l;
            right = r;
            top = t;
            bottom = b;
        }

        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    [CreateAssetMenu(menuName ="TextureSheet")]
    public class TextureSheet : ScriptableObject
    {
        //Filed---------------------------------------------------
        [SerializeField]
        private Texture2D m_Texture;
        [SerializeField,HideInInspector]
        private int m_nMaxRow;
        [SerializeField, HideInInspector]
        private int m_nMaxColumn;

        [SerializeField, HideInInspector]
        private float m_fCellWidth;
        [SerializeField, HideInInspector]
        private float m_fCellHeight;

        //property---------------------------------------------------
        public int MaxRow { get { return m_nMaxRow; } }
        public int MaxColumn { get { return m_nMaxColumn; } }
        public Texture2D Tex { get { return m_Texture; } }


        //function-----------------------------------------------------

        public void SetMaxRow(int row)
        {
            m_nMaxRow = row;
            m_fCellHeight = 1.0f / row;
        }
        public void SetMaxColumn(int column)
        {
            m_nMaxColumn = column;
            m_fCellWidth = 1.0f / column;
        }

        public Frame GetCoord(int row, int column)
        {
            if (row > m_nMaxRow || column > m_nMaxColumn || row<0 || column<0)
            {
                return null;
            }
            
            Frame frame = new Frame();

            frame.top = (m_nMaxRow - row) * m_fCellHeight;
            frame.left = column * m_fCellWidth;
            frame.bottom = (m_nMaxRow - row - 1) * m_fCellHeight;
            frame.right = (column + 1) * m_fCellWidth;

            return frame;
        }
        public Frame GetCoord(int index)
        {
            int row = index / m_nMaxColumn;
            int column = index % m_nMaxColumn;

            return GetCoord(row, column);
        }
    }
}
