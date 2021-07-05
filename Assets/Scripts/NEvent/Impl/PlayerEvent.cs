using UnityEngine;

using Assets.Scripts.NCharacter;

namespace Assets.Scripts.NEvent
{
    //Event: Player's Chunk Location has Changed
    public class E_Player_Spawned : EventBase<E_Player_Spawned>
    {
        public Vector3 SpawnPos { set; get; }
        public Character Player { set; get; }

        public E_Player_Spawned(Vector3 _SpawnPos,Character _player)
        {
            this.Priority = enPriority.Highest;

            SpawnPos = _SpawnPos;
            Player = _player;
        }
    }

    class E_Player_LeaveChunk : EventBase<E_Player_LeaveChunk>
    {
        public Vector2Int Offset;

        public E_Player_LeaveChunk(Vector2Int _offset)
        {
            this.Priority = enPriority.Highest;

            Offset = _offset;
        }
    }
}
