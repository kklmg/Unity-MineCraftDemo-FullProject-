using UnityEngine;

using Assets.Scripts.NMesh;
using Assets.Scripts.NData;

namespace Assets.Scripts.NWorld
{
    public interface IBlock
    {
        byte ID { get; }
        string Name { get; }

        Sprite Icon { get; }

        bool IsSpecial { get; }
        bool IsOpaque { get; }
        bool IsObstacle { get;}

        bool IsLeftMeshExist { get; }
        bool IsRigthMeshExist { get; }
        bool IsUpMeshExist { get; }
        bool IsDownMeshExist { get; }
        bool IsFrontMeshExist { get; }
        bool IsBackMeshExist { get; }

        void Init(TextureSheet TexSheet);

        DynamicMesh GetLeftMesh(int x = 0, int y = 0, int z = 0);
        DynamicMesh GetRightMesh(int x = 0, int y = 0, int z = 0);
        DynamicMesh GetUpMesh(int x = 0, int y = 0, int z = 0);
        DynamicMesh GetDownMesh(int x = 0, int y = 0, int z = 0);
        DynamicMesh GetFrontMesh(int x = 0, int y = 0, int z = 0);
        DynamicMesh GetBackMesh(int x = 0, int y = 0, int z = 0);

        //ushort GetLeftTextureID();
        //ushort GetRightTextureID();
        //ushort GetUpTextureID();
        //ushort GetDownTextureID();
        //ushort GetFrontTextureID();
        //ushort GetBackTextureID();

        DynamicMesh GetAllMesh(int x, int y, int z);
    }
}
