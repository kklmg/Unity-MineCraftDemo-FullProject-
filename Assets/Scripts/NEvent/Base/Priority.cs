using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.NEvent
{
    public enum enPriority
    {
        Highest = 0,
        level_1,
        level_2,
        level_3,
        level_4,
        level_5,
        level_6,
        level_7,
        level_8,
        Lowest = 9,

        Count=10,
        Default = Lowest,

        NotImportent,
        UnKnown,
    }

    //static class EPriorityMethods
    //{
    //    public static int LevelCount(this enPriority p)
    //    {
    //    }
    //}
}
