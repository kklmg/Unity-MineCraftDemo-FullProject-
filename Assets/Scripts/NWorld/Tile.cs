using System;

using UnityEngine;

using Assets.Scripts.NMesh;

namespace Assets.Scripts.NWorld
{
    [System.Serializable]
    public class Tile
    {
        public DynamicMeshScObj OriginMesh;

        [SerializeField]
        private int m_TexID;

        private DynamicMesh m_Mesh;
        private DynamicMesh m_ClonedMesh;

        public void Init(TextureSheet TexSheet)
        {
            m_Mesh = OriginMesh.GetClonedMesh();
            m_Mesh.SetUV_quad(TexSheet.GetCoord(m_TexID));
        }

        public DynamicMesh GetTransMesh(int x, int y, int z)
        {
            m_ClonedMesh = m_Mesh.Clone();
            m_ClonedMesh.Translate(new Vector3(x, y, z));

            return m_ClonedMesh;
        }
    }

}
