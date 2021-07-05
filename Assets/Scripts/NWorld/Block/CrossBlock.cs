using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.NMesh;
using UnityEngine;

namespace Assets.Scripts.NWorld
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "CrossBlock")]
    public class CrossBlock : ScriptableObject, IBlock
    {
        //Field------------------------------------------
        [SerializeField]
        private byte m_ID;

        [SerializeField]
        private Sprite m_Icon;

        [SerializeField]
        private bool m_IsOpaque = true;

        [SerializeField]
        private bool m_IsObstacle = false;

        [SerializeField]
        private Tile m_LeftToRight;

        [SerializeField]
        private Tile m_FrontToBack;


        public byte ID { get { return m_ID; } }
        public string Name { get { return name; } }
        public Sprite Icon { get { return m_Icon; } }

        public bool IsSpecial { get { return true; } }
        public bool IsOpaque { get { return m_IsOpaque; } }
        public bool IsObstacle { get { return m_IsObstacle; } }

        public bool IsLeftMeshExist { get { return false; } }
        public bool IsRigthMeshExist { get { return false; } }
        public bool IsUpMeshExist { get { return false; } }
        public bool IsDownMeshExist { get { return false; } }
        public bool IsFrontMeshExist { get { return false; } }
        public bool IsBackMeshExist { get { return false; } }


        public void Init(TextureSheet TexSheet)
        {
            m_LeftToRight.Init(TexSheet);
            m_FrontToBack.Init(TexSheet);
        }


        public DynamicMesh GetLeftMesh(int x = 0, int y = 0, int z = 0)
        {
            return null;
        }

        public DynamicMesh GetRightMesh(int x = 0, int y = 0, int z = 0)
        {
            return null;
        }

        public DynamicMesh GetUpMesh(int x = 0, int y = 0, int z = 0)
        {
            return null;
        }

        public DynamicMesh GetDownMesh(int x = 0, int y = 0, int z = 0)
        {
            return null;
        }

        public DynamicMesh GetFrontMesh(int x = 0, int y = 0, int z = 0)
        {
            return null;
        }

        public DynamicMesh GetBackMesh(int x = 0, int y = 0, int z = 0)
        {
            return null;
        }

        public DynamicMesh GetAllMesh(int x, int y, int z)
        {
            DynamicMesh TempMesh = new DynamicMesh();

            TempMesh.Add(m_LeftToRight.GetTransMesh(x, y, z));
            TempMesh.Add(m_FrontToBack.GetTransMesh(x, y, z));
            
            return TempMesh;
        }
    }
}
