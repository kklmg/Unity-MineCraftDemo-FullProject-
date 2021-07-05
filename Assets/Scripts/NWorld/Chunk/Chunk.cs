using System.Collections;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.Singleton;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NData;

namespace Assets.Scripts.NWorld
{
    public class Chunk : MonoBehaviour
    {
        //Field
        //------------------------------------------------------------------------

        public GameObject m_Prefab_Section;

        [SerializeField]
        private ChunkInWorld m_ChunkinWorld;

        [SerializeField]
        private Vector2Int m_Coord;

        private IWorld m_World;

        [SerializeField]
        private ChunkHeightMap m_HeightMap;

        //[SerializeField]
        private Section[] m_arrSections;  //Sections

        public Vector2Int Loc{ get { return m_ChunkinWorld.Value; } }
        public ChunkHeightMap HeightMap { get { return m_HeightMap; } }
        //Public Function
        //------------------------------------------------------------------------

        public Section GetSection(SectionInChunk slot)
        {
            if (slot.Value < 0 || slot.Value >= m_World.Chunk_Height) return null;

            return m_arrSections[slot.Value];
        }

        public bool TryGetSection(SectionInChunk slot, out Section Sec)
        {
            Sec = default(Section);

            slot.Value = slot.Value % m_World.Chunk_Height;

            if (slot.Value < 0 || slot.Value > m_World.Chunk_Height)
            {
                return false;
            }
            else
            {
                Sec = m_arrSections[slot.Value];
                return true;
            }

        }

        public Section CreateBlankSection(SectionInChunk slot)
        {
            if (slot.Value < 0 || slot.Value >= m_World.Chunk_Height) return null;

            //make a Section instance
            m_arrSections[slot.Value] =
                Instantiate(m_Prefab_Section, transform).GetComponent<Section>();
            //Init Section
            m_arrSections[slot.Value].SetLocation
                (new SectionInWorld(m_ChunkinWorld.Value.x, slot.Value, m_ChunkinWorld.Value.y));

            m_arrSections[slot.Value].CreateEmptyVoxel();

            return m_arrSections[slot.Value];
        }

        public bool GetGroundHeight(int blkx, int blkz, int CurY, out float GroundY)
        {
            CurY -= 1;

            int SecID;
            int blky;
            Section CurSec;
            IBlock CurBlock;

            while (CurY > -1)
            {
                //Get Currect Section
                SecID = CurY / m_World.Chunk_Height;
                CurSec = m_arrSections[SecID];

                //Block Height
                blky = CurY % m_World.Chunk_Height;

                if (CurSec == null)
                {
                    CurY -= blky + 1;
                    continue;
                }

                while (blky > -1)
                {
                    CurBlock = CurSec.Voxel.GetBlock(new BlockInSection(blkx, blky, blkz, m_World));

                    if (CurBlock != null && CurBlock.IsObstacle)
                    {
                        GroundY = (blky + 1) + SecID * m_World.Chunk_Height;
                        return true;
                    }
                    blky -= 1;
                    CurY -= 1;
                }
            }
            GroundY = default(float);
            return false;
        }

        //Unity Funciton
        //------------------------------------------------------------------------------
        public bool IsBlockDataGenerated = false;
        private void Awake()
        {
            m_World = Locator<IWorld>.GetService();
            m_arrSections = new Section[m_World.Chunk_Height];
        }

        public void SetLocation(ChunkInWorld chunkInWorld)
        {
            GSW.RestartTimer();
            //Set Location
            m_ChunkinWorld = chunkInWorld;
            transform.localPosition = m_ChunkinWorld.ToCoord3D(m_World);

            //Set Name
            transform.name = "Chunk" + "[" + m_ChunkinWorld.Value.x + "]" + "[" + m_ChunkinWorld.Value.y + "]";

            //Compute Unity Position
            m_Coord = chunkInWorld.ToCoord2DInt(m_World);

            //create Height map

            m_HeightMap
              = MonoSingleton<GameSystem>.Instance.WorldMngIns.GetComponent<MapMaker>().GetBlendedHeightMap(m_ChunkinWorld.Value);
           // = MonoSingleton<GameSystem>.Instance.WorldMngIns.GetComponent<BiomeMng>().MakeHeightMap(m_ChunkinWorld.Value);
            //Create Sections
            InitAllSections();

            //StartCoroutine("CreCreateAllSections_Corou");
        }

        //create instance of Section
        private void InitSection(int slot_y)
        {
            int sec_minHeight = slot_y * m_World.Chunk_Height;

            //case1: section lowest height > terrain tallest height
            //case2: section tallest height < terrain lowetst height
            if (sec_minHeight > m_HeightMap.MaxAltitude 
                /*|| sec_minHeight + m_refWorld.Chunk_Height < m_HeightMap.MinHeight*/)
            {
                if (m_arrSections[slot_y] != null)
                {
                    m_arrSections[slot_y].gameObject.SetActive(false);
                    Destroy(m_arrSections[slot_y].gameObject);
                    m_arrSections[slot_y] = null;
                }
                return;
            }

            if (m_arrSections[slot_y] == null)
            {
                //make a Section instance
                m_arrSections[slot_y] =
                    Instantiate(m_Prefab_Section, transform).GetComponent<Section>();
            }

            m_arrSections[slot_y].SetLocation(new SectionInWorld(m_ChunkinWorld.Value.x, slot_y, m_ChunkinWorld.Value.y));
        }

        private void InitAllSections()
        {
            for (int i = 0; i < m_World.Chunk_Height; ++i)
            {
                InitSection(i);
            }
        }

        public void GenerateBlocks()
        {
            for (int i = 0; i < m_World.Chunk_Height; ++i)
            {
                if (m_arrSections[i] != null)
                    m_arrSections[i].CreateVoxel();
            }
            IsBlockDataGenerated = true;
        }


        public void BuildMesh()
        {
            for (int i = 0; i < m_World.Chunk_Height; ++i)
            {
                if (m_arrSections[i] != null)
                    m_arrSections[i].Voxel.BuildMesh();
            }
        }

        public void ClearUnityMesh()
        {
            for (int i = 0; i < m_World.Chunk_Height; ++i)
            {
                if (m_arrSections[i] != null)
                    m_arrSections[i].ClearMesh();
            }
        }
    }
}


