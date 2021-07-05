using System.Collections.Generic;

using UnityEngine;

using Assets.Scripts.NData;

namespace Assets.Scripts.NWorld
{
    [System.Serializable]
    public class SaveHelper_Section
    {
        [SerializeField]
        private Dictionary<Vector3Int, int> m_Dic;
        private List<BlockChange> m_Changes;

        public SaveHelper_Section(SectionChange Data)
        {
            m_Dic = new Dictionary<Vector3Int, int>();
            m_Changes = Data.Data;
            int ListIndex = 0;
            foreach (var kv in m_Changes)
            {
                m_Dic.Add(kv.Slot, ListIndex++);
            }
        }

        public void Modify(BlockInSection slot, byte blockId)
        {
            if (m_Dic.ContainsKey(slot.Value))
            {
                m_Changes[m_Dic[slot.Value]]
                    = new BlockChange(slot.Value, blockId);
                //Debug.Log("Block has Modifid");
            }
            else
            {
                m_Dic.Add(slot.Value, m_Changes.Count);
                m_Changes.Add(new BlockChange(slot.Value, blockId));
                //Debug.Log("Block has Added");
            }
        }

        public void ApplyModification(Section section, IWorld world)
        {
            foreach (var Change in m_Changes)
            {
                section.Voxel.SetBlock(new BlockInSection(Change.Slot, world), Change.Value, false);
            }
        }

        public int Count { get { return m_Changes.Count; } }
    }
}
