using System;

using UnityEngine;

namespace Assets.Scripts.NData
{
    [Serializable]
    public struct GameTime
    {
        public void Assign(DateTime time)
        {
            Year = time.Year;
            Month = time.Month;
            Day = time.Day;
            Hour = time.Hour;
            Minute = time.Minute;
            Second = time.Second;
        }

        public override string ToString()
        {
            return new System.DateTime(Year, Month, Day, Hour, Minute, Second).ToString();
        }

        [SerializeField]
        int Year;
        [SerializeField]
        int Month;
        [SerializeField]
        int Day;
        [SerializeField]
        int Hour;
        [SerializeField]
        int Minute;
        [SerializeField]
        int Second;
    }
}
