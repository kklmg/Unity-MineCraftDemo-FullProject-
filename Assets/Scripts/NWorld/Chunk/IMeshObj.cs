

namespace Assets.Scripts.NWorld
{
    interface IMeshObj
    {
        void ClearMesh();

        void GenerateBlocks();

        void BuildMeshInstantly();

        void BuildMeshInBackground();

        void RunBuildMeshCoro();
    }
}
