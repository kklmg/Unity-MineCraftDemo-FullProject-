using System.Collections.Generic;

using UnityEngine;

using Assets.Scripts.NNoise;
using Assets.Scripts.NData;


namespace Assets.Scripts.NWorld
{
    public class World : MonoBehaviour, IWorld
    {
        //Filed
        //--------------------------------------------------------------------
        //Section Size 
        [SerializeField]
        private ushort m_Section_Width = 16;
        [SerializeField]
        private ushort m_Section_Height = 16;
        [SerializeField]
        private ushort m_Section_Depth = 16;

        //chunk Height 
        [SerializeField]
        private ushort m_Chunk_Height = 16;

        [SerializeField]
        private BlockPalette m_BlockPalette;

        [SerializeField]
        private TextureSheet m_TextureSheet;

        //Property
        //--------------------------------------------------------------------
        public ushort Section_Width { get { return m_Section_Width; } }
        public ushort Section_Height { get { return m_Section_Height; } }
        public ushort Section_Depth { get { return m_Section_Depth; } }
        public ushort Chunk_Height { get { return m_Chunk_Height; } }
        public WorldEntity Entity { get; private set; }
       
        public BlockPalette BlkPalette { get { return m_BlockPalette; } }
        public TextureSheet TexSheet { get { return m_TextureSheet; } }

        //Public Function
        //--------------------------------------------------------------------
        public void Init(string Seed)
        {          
            //Init blockPalette
            m_BlockPalette.Init();
        }

        //unity function
        //--------------------------------------------------------------------
        private void Awake()
        {
            Entity = GetComponentInChildren<WorldEntity>();
        }

        public Bounds GetBound(Vector3 Coord)
        {
            Bounds Temp = new Bounds();
            Vector3 vt = new Vector3(
                (Coord.x >= 0 ? (int)Coord.x : (int)Coord.x - 1),
                (Coord.y >= 0 ? (int)Coord.y : (int)Coord.y - 1),
                (Coord.z >= 0 ? (int)Coord.z : (int)Coord.z - 1));

            Temp.SetMinMax(vt, vt + Vector3.one);
            return Temp;
        }
    }
}