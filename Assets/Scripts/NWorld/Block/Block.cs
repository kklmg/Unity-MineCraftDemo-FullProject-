using UnityEngine;

using Assets.Scripts.NMesh;
using Assets.Scripts.NData;

namespace Assets.Scripts.NWorld
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "block")]
    public class Block : ScriptableObject, IBlock
    {
        //Field------------------------------------------
        [SerializeField]
        private byte m_ID;

        [SerializeField]
        private Sprite m_Icon;

        [SerializeField]
        private bool m_IsOpaque = true;

        [SerializeField]
        private bool m_IsObstacle = true;

        [SerializeField]
        private Tile m_UpTile;
        [SerializeField]
        private Tile m_DownTile;
        [SerializeField]
        private Tile m_LeftTile;
        [SerializeField]
        private Tile m_RightTile;
        [SerializeField]
        private Tile m_FrontTile;
        [SerializeField]
        private Tile m_BackTile;

        public byte ID { get { return m_ID; } }
        public string Name { get { return name; } }
        public Sprite Icon { get { return m_Icon; } }

        public bool IsSpecial { get { return false; } }
        public bool IsOpaque { get { return m_IsOpaque; } }
        public bool IsObstacle { get { return m_IsObstacle; } }

        public bool IsLeftMeshExist { get { return m_LeftTile != null; } }
        public bool IsRigthMeshExist { get { return m_RightTile != null; } }
        public bool IsUpMeshExist { get { return m_UpTile != null; } }
        public bool IsDownMeshExist { get { return m_DownTile != null; } }
        public bool IsFrontMeshExist { get { return m_FrontTile != null; } }
        public bool IsBackMeshExist { get { return m_BackTile != null; } }

        public Tile UpTile { get { return m_UpTile; } }
        public Tile DownTile { get { return m_DownTile; } }
        public Tile LeftTile { get { return m_LeftTile; } }
        public Tile RightTile { get { return m_RightTile; } }
        public Tile FrontTile { get { return m_FrontTile; } }
        public Tile BackTile { get { return m_BackTile; } }


        public void Init(TextureSheet TexSheet)
        {
            m_LeftTile.Init(TexSheet);
            m_RightTile.Init(TexSheet);
            m_UpTile.Init(TexSheet);
            m_DownTile.Init(TexSheet);
            m_FrontTile.Init(TexSheet);
            m_BackTile.Init(TexSheet);
        }


        public DynamicMesh GetLeftMesh(int x = 0, int y = 0, int z = 0)
        {
            //if (m_LeftTile == null) return null;
            return m_LeftTile.GetTransMesh(x, y, z);
        }

        public DynamicMesh GetRightMesh(int x = 0, int y = 0, int z = 0)
        {
            //if (m_RightTile == null) return null;
            return m_RightTile.GetTransMesh(x, y, z);
        }

        public DynamicMesh GetUpMesh(int x = 0, int y = 0, int z = 0)
        {
            //if (m_UpTile == null) return null;
            return m_UpTile.GetTransMesh(x, y, z);
        }

        public DynamicMesh GetDownMesh(int x = 0, int y = 0, int z = 0)
        {
            //if(m_DownTile == null) return null;
            return m_DownTile.GetTransMesh(x, y, z);
        }

        public DynamicMesh GetFrontMesh(int x = 0, int y = 0, int z = 0)
        {
            //if (m_FrontTile == null) return null;
            return m_FrontTile.GetTransMesh(x, y, z);
        }

        public DynamicMesh GetBackMesh(int x = 0, int y = 0, int z = 0)
        {
            //if (m_BackTile == null) return null;
            return m_BackTile.GetTransMesh(x, y, z);
        }

        public DynamicMesh GetAllMesh(int x, int y, int z)
        {
            DynamicMesh TempMesh = new DynamicMesh();

            TempMesh.Add(GetUpMesh(x, y, z));
            TempMesh.Add(GetDownMesh(x, y, z));
            TempMesh.Add(GetLeftMesh(x, y, z));
            TempMesh.Add(GetRightMesh(x, y, z));
            TempMesh.Add(GetFrontMesh(x, y, z));
            TempMesh.Add(GetBackMesh(x, y, z));

            return TempMesh;
        }
    }



}
