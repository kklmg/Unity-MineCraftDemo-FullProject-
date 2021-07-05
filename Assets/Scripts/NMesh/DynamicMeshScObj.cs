using System;
using UnityEngine;

namespace Assets.Scripts.NMesh
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "DynamicMesh")]
    public class DynamicMeshScObj : ScriptableObject
    {
        [SerializeField]
        private DynamicMesh m_Mesh;

        public DynamicMesh Mesh{ get { return m_Mesh; } }

        public DynamicMesh GetClonedMesh()
        {
            return m_Mesh.Clone();
        }
    }
}
