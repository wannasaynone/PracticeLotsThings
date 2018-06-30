using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager {

    private class Timer
    {
        public float Time;
        public Action Action;
    }

    private static Dictionary<long, Timer> m_timers = new Dictionary<long, Timer>();
    private static List<long> m_waitForRemoveTimers = new List<long>();
    private static long m_currentID = 0;

    public static long Schedule(float time, Action action)
    {
        if (m_currentID + 1 > long.MaxValue)
        {
            m_currentID = 0;
        }
        else
        {
            m_currentID++;
        }

        m_timers.Add(m_currentID, new Timer() { Time = time, Action = action });

        return m_currentID;
    }

    public static void Tick(float deltaTime)
    {
        foreach (KeyValuePair<long, Timer> kvp in m_timers)
        {
            kvp.Value.Time -= deltaTime;
            if (kvp.Value.Time <= 0)
            {
                kvp.Value.Action();
                m_waitForRemoveTimers.Add(kvp.Key);
            }
        }

        for(int _timerIndex = 0; _timerIndex < m_waitForRemoveTimers.Count; _timerIndex++)
        {
            m_timers.Remove(m_waitForRemoveTimers[_timerIndex]);
        }

        if(m_waitForRemoveTimers.Count > 0)
        {
            m_waitForRemoveTimers.Clear();
        }
    }

    public static void AddTime(long id, float time)
    {
        if(m_timers.ContainsKey(id))
        {
            m_timers[id].Time += time;
        }
    }

}
