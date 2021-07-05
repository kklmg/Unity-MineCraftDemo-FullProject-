using UnityEngine;
using System.Collections;
using System.Threading;

using Assets.Scripts.NData;
using Assets.Scripts.NMesh;
using Assets.Scripts.NGlobal.ServiceLocator;

namespace Assets.Scripts.NWorld
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]

    public class Section : MonoBehaviour
    {
        //Field----------------------------------------      
        private IWorld m_World;

        //Location Data
        [SerializeField]
        private SectionInWorld m_SecInWorld;

        //unity component
        private MeshFilter m_MeshFilter;
        private MeshRenderer m_MeshRenderer;
        
        Chunk m_Chunk; //the chunk this section belong to
        
        Voxel m_Voxel;

        //property--------------------------------------
        public Voxel Voxel { get { return m_Voxel; } }
        public SectionInWorld SectionInWorld { get { return m_SecInWorld; } set { m_SecInWorld = value; } }

        //unity function-----------------------------------------
        private void Awake()
        {
            m_World = Locator<IWorld>.GetService();

            //cache unity component
            m_MeshFilter = gameObject.GetComponent<MeshFilter>();
            m_MeshRenderer = gameObject.GetComponent<MeshRenderer>();

            //instantiate voxel
            m_Voxel = new Voxel(m_World);
        }

        private void Update()
        {
            m_Voxel.TryGetUnityMesh(m_MeshFilter);
        }

        //Public function
        //------------------------------------------------------------------------------------------
        public void SetLocation(SectionInWorld sectionInWorld)
        {
            m_Chunk = GetComponentInParent<Chunk>();

            //Init Location
            m_SecInWorld = sectionInWorld;    
            transform.localPosition = new Vector3(0, m_SecInWorld.Value.y * m_World.Chunk_Height, 0);

            //set name
            transform.name = "Section" + '[' + m_SecInWorld.Value.y + ']';
        }

        public void CreateEmptyVoxel()
        {
            m_Voxel.MakeEmpty();
        }

        public void CreateVoxel()
        {
            ChunkHeightMap heightMap = m_Chunk.HeightMap;
           // LayerData layerData = m_Chunk.GetBiome().Layer;

            int BaseAltitude = m_SecInWorld.ToSectionInChunk().Value * m_World.Section_Height;

            m_Voxel.Make(heightMap,BaseAltitude);
        }

        public void ClearMesh()
        {
            m_MeshFilter.mesh.Clear();
            m_MeshFilter.mesh = null;
        }

    }
}