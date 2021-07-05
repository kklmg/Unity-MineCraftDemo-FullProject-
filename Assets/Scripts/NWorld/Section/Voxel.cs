using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

using Assets.Scripts.NMesh;
using Assets.Scripts.NData;



namespace Assets.Scripts.NWorld
{
    public class Voxel
    {
        private IWorld m_World;
        private byte[,,] m_Voxel;

        private DynamicMesh m_DynamicMesh;

        private object m_LockObj;

        private bool IsDirty = false;

        public Voxel(IWorld _world)
        {
            m_World = _world;
            m_Voxel = new byte[_world.Section_Width, _world.Section_Height, _world.Section_Depth];
            m_DynamicMesh = new DynamicMesh();
            m_LockObj = new object();
        }


        public bool TryGetUnityMesh(MeshFilter meshFilter, bool IfClearDynamicMesh = true)
        {
            //lock (m_LockObj)
            {
                if (IsDirty)
                {
                    IsDirty = false;

                    //Get UnityMesh
                    meshFilter.mesh = m_DynamicMesh.GenerateUnityMesh();

                    //Clear Dynamic Data
                    if (IfClearDynamicMesh)
                        m_DynamicMesh.Clear();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



        public IBlock GetBlock(int x, int y, int z)
        {
            return (x > -1 && x < m_World.Section_Width
                 && y > -1 && y < m_World.Chunk_Height
                 && z > -1 && z < m_World.Section_Depth)

                 ? m_World.BlkPalette[m_Voxel[x, y, z]] : null;
        }
        public IBlock GetBlock(BlockInSection blkInSec)
        {
            return m_World.BlkPalette[m_Voxel[blkInSec.x, blkInSec.y, blkInSec.z]];
        }
        public byte GetBlockID(BlockInSection blkInSec)
        {
            return m_Voxel[blkInSec.x, blkInSec.y, blkInSec.z];
        }

        public void SetBlock(BlockInSection blkInSec, byte blkID, bool IfUpdateMesh = true)
        {
            m_Voxel[blkInSec.x, blkInSec.y, blkInSec.z] = blkID;
            if (IfUpdateMesh) BuildMesh();
        }

        public void Make(ChunkHeightMap heightMap,int BaseAltitude)
        {
            //lock (m_LockObj)
            {
                //init blockType 
                int x, y, z;
                Block TargetBlock;
                //Biome biome;

                for (x = 0; x < m_World.Section_Width; x++)
                {
                    for (y = 0; y < m_World.Section_Height; y++)
                    {
                        for (z = 0; z < m_World.Section_Depth; z++)
                        {
                            TargetBlock = heightMap[x, z].Layer == null
                                ? null : heightMap[x, z].Layer.GetBlock(y + BaseAltitude, heightMap[x, z].Height);

                            m_Voxel[x, y, z] = (TargetBlock == null) ? byte.MinValue : TargetBlock.ID;
                        }
                    }
                }
            }
        }

        public void MakeEmpty()
        {
            //lock (m_LockObj)
            {
                int x, y, z;
                for (x = 0; x < m_World.Section_Width; x++)
                {
                    for (y = 0; y < m_World.Section_Height; y++)
                    {
                        for (z = 0; z < m_World.Section_Depth; z++)
                        {
                            m_Voxel[x, y, z] = 0;
                        }
                    }
                }
            }
        }

        public void AssignBLockDatas(ref byte[,,] Datas)
        {
            m_Voxel = Datas;
        }

        //build mesh 
        public void BuildMesh()
        {
            //lock (m_LockObj)
            {
                //Clear Current Mesh 
                m_DynamicMesh.Clear();

                IBlock CurBlk, AdjBlk;

                int x, y, z;
                for (x = 0; x < m_World.Section_Width; x++)
                {
                    for (y = 0; y < m_World.Section_Height; y++)
                    {
                        for (z = 0; z < m_World.Section_Depth; z++)
                        {
                            //get block type
                            CurBlk = m_World.BlkPalette[m_Voxel[x, y, z]];

                            if (CurBlk == null) continue;

                            //case: Block is not opaque
                            if (!CurBlk.IsOpaque)
                            {
                                m_DynamicMesh.Add(CurBlk.GetAllMesh(x, y, z));
                            }
                            else
                            {
                                //Check Up side
                                AdjBlk = GetBlock(x, y + 1, z);
                                if (AdjBlk == null || !AdjBlk.IsOpaque || !AdjBlk.IsDownMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetUpMesh(x, y, z));
                                }

                                //Check Down side
                                AdjBlk = GetBlock(x, y - 1, z);
                                if (AdjBlk == null || !AdjBlk.IsOpaque || !AdjBlk.IsUpMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetDownMesh(x, y, z));
                                }

                                //Check Left side
                                AdjBlk = GetBlock(x - 1, y, z);
                                if (AdjBlk == null || !AdjBlk.IsOpaque || !AdjBlk.IsRigthMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetLeftMesh(x, y, z));
                                }

                                //Check Right side
                                AdjBlk = GetBlock(x + 1, y, z);
                                if (AdjBlk == null || !AdjBlk.IsOpaque || !AdjBlk.IsLeftMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetRightMesh(x, y, z));
                                }

                                //Check Front side
                                AdjBlk = GetBlock(x, y, z + 1);
                                if (AdjBlk == null || !AdjBlk.IsOpaque || !AdjBlk.IsDownMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetFrontMesh(x, y, z));
                                }

                                //Check Back side
                                AdjBlk = GetBlock(x, y, z - 1);
                                if (AdjBlk == null || !AdjBlk.IsOpaque || !AdjBlk.IsDownMeshExist)
                                {
                                    m_DynamicMesh.Add(CurBlk.GetBackMesh(x, y, z));
                                }
                            }
                        }//z-loop
                    }//y-loop
                }//x-loop
                IsDirty = true;
            }
        }



        public void CopyFrom(Voxel voxel)
        {
            m_World = voxel.m_World;
            m_Voxel = voxel.m_Voxel;
        }

        public Voxel Clone()
        {
            Voxel ClonedVoxel = new Voxel(m_World);
            ClonedVoxel.AssignBLockDatas(ref m_Voxel);

            return ClonedVoxel;
        }
    }
}