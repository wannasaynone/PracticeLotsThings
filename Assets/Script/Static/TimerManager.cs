﻿using System;
using System.Collections.Generic;

namespace PracticeLotsThings
{
    public class TimerManager
    {
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
            List<long> _ids = new List<long>(m_timers.Keys);
            for (int i = 0; i < _ids.Count; i++)
            {
                m_timers[_ids[i]].Time -= deltaTime;
                if (m_timers[_ids[i]].Time <= 0)
                {
                    if (m_timers[_ids[i]].Action != null)
                    {
                        m_timers[_ids[i]].Action();
                    }
                    m_waitForRemoveTimers.Add(_ids[i]);
                }
            }

            for (int i = 0; i < m_waitForRemoveTimers.Count; i++)
            {
                m_timers.Remove(m_waitForRemoveTimers[i]);
            }

            if (m_waitForRemoveTimers.Count > 0)
            {
                m_waitForRemoveTimers.Clear();
            }
        }

        public static void AddTime(long id, float time)
        {
            if (m_timers.ContainsKey(id))
            {
                m_timers[id].Time += time;
            }
        }
    }
}
