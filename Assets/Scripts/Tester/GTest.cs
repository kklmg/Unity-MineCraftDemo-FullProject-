using UnityEngine;

public static class GSW
{
    static bool IsActive = true;

    static System.Diagnostics.Stopwatch m_Stopwatch;

    static public void ShowElapsedTime(string str)
    {
        if (IsActive)
        {
            Debug.Log(str + ": " + m_Stopwatch.ElapsedTicks.ToString());
        }
    }

    static public void ShowElapsedTimeAndRestart(string str)
    {
        if (IsActive)
        {
            Debug.Log(str + ": " + m_Stopwatch.ElapsedTicks.ToString());
            m_Stopwatch.Restart();
        }
    }



    static public void StartTimer()
    {
        if (IsActive)
        {
            if (m_Stopwatch == null)
            {
                m_Stopwatch = new System.Diagnostics.Stopwatch();
            }
            m_Stopwatch.Start();
        }
    }
    static public void RestartTimer()
    {
        if (IsActive)
        {
            if (m_Stopwatch == null)
            {
                m_Stopwatch = new System.Diagnostics.Stopwatch();
            }
            m_Stopwatch.Restart();
        }
    }
}
