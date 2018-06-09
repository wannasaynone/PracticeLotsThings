using System;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;

public class GameDataManager {

    private static Dictionary<Type, IGameData[]> m_gameDatas = new Dictionary<Type, IGameData[]>();

    public static void LoadGameData<T>(string path) where T : IGameData
    {
        T[] datas = JsonReader.Deserialize<T[]>(Resources.Load<TextAsset>(path).text);
        if(m_gameDatas.ContainsKey(typeof(T)))
        {
            m_gameDatas[typeof(T)] = new IGameData[datas.Length];
            for(int i = 0; i < datas.Length; i++)
            {
                m_gameDatas[typeof(T)][i] = datas[i];
            }
        }
        else
        {
            IGameData[] _gameDatas = new IGameData[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                _gameDatas[i] = datas[i];
            }
            m_gameDatas.Add(typeof(T), _gameDatas);
        }
    }

    public static T GetGameData<T>(int id) where T : IGameData
    {
        if (m_gameDatas.ContainsKey(typeof(T)))
        {
            for (int i = 0; i < m_gameDatas[typeof(T)].Length; i++)
            {
                if (m_gameDatas[typeof(T)][i].ID == id)
                {
                    return (T)m_gameDatas[typeof(T)][i];
                }
            }
        }

        return default(T);
    }

    public static T[] GetAllGameData<T>() where T : IGameData
    {
        if (m_gameDatas.ContainsKey(typeof(T)))
        {
            T[] _gameDatas = new T[m_gameDatas[typeof(T)].Length];
            for (int i = 0; i < m_gameDatas[typeof(T)].Length; i++)
            {
                _gameDatas[i] = (T)m_gameDatas[typeof(T)][i];
            }
            return _gameDatas;
        }

        return default(T[]);
    }

}
