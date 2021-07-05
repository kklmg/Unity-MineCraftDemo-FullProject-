using UnityEngine;

using System.Collections.Generic;

using Assets.Scripts.NData;

namespace Assets.Scripts.NWorld
{
    public class SaveHelper_Chunk
    {      
        private Dictionary<int, SaveHelper_Section> m_Helpers;

        [SerializeField]
        private List<SectionChange> m_Changes;

        public SaveHelper_Chunk(ChunkChange Change)
        {
            m_Helpers = new Dictionary<int, SaveHelper_Section>();
            m_Changes = Change.Data;
            
            foreach (var cha in m_Changes)
            {
                m_Helpers.Add(cha.Height, new SaveHelper_Section(cha));
            }
        }

        public void Modify(SectionInChunk Loc, BlockInSection slot, byte blockId)
        {
            if (m_Helpers.TryGetValue(Loc.Value,out var SectionHelper))
            {
                SectionHelper.Modify(slot, blockId);
            }
            else
            {
                m_Changes.Add(new SectionChange(Loc.Value));
                SectionHelper = new SaveHelper_Section(m_Changes[m_Changes.Count - 1]);

                m_Helpers.Add(Loc.Value, SectionHelper);
                SectionHelper.Modify(slot, blockId);
            }
        }

        public void ApplyModification(Chunk _chunk, IWorld world)
        {
            Section Sec;
            SectionInChunk SecInchunk;
            foreach (var helper in m_Helpers)
            {
                SecInchunk = new SectionInChunk(helper.Key);
                Sec = _chunk.GetSection(SecInchunk);
                if (Sec == null)
                {
                    Sec = _chunk.CreateBlankSection(SecInchunk);
                }
                if (Sec == null)
                {
                    Debug.LogError("over bound");
                    continue;
                }
                helper.Value.ApplyModification(Sec, world);

            }        
        }

        public int Count
        {
            get
            {
                int temp = 0;
                foreach (var helper in m_Helpers)
                {
                    temp += m_Helpers.Count;
                }
                return temp;
            }
        }
    }
}
