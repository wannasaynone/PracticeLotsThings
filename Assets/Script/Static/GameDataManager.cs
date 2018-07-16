using System;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;

public class GameDataManager {

    private static Dictionary<Type, IGameData[]> m_gameData = new Dictionary<Type, IGameData[]>();

    public static void LoadGameData<T>(string path) where T : IGameData
    {
        T[] data = JsonReader.Deserialize<T[]>(Resources.Load<TextAsset>(path).text);
        if(m_gameData.ContainsKey(typeof(T)))
        {
            m_gameData[typeof(T)] = new IGameData[data.Length];
            for(int i = 0; i < data.Length; i++)
            {
                m_gameData[typeof(T)][i] = data[i];
            }
        }
        else
        {
            IGameData[] _gameData = new IGameData[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                _gameData[i] = data[i];
            }
            m_gameData.Add(typeof(T), _gameData);
        }
    }

    public static T GetGameData<T>(int id) where T : IGameData
    {
        if (m_gameData.ContainsKey(typeof(T)))
        {
            for (int i = 0; i < m_gameData[typeof(T)].Length; i++)
            {
                if (m_gameData[typeof(T)][i].ID == id)
                {
                    return (T)m_gameData[typeof(T)][i];
                }
            }
        }

        return default(T);
    }

    public static T[] GetAllGameData<T>() where T : IGameData
    {
        if (m_gameData.ContainsKey(typeof(T)))
        {
            T[] _gameDatas = new T[m_gameData[typeof(T)].Length];
            for (int i = 0; i < m_gameData[typeof(T)].Length; i++)
            {
                _gameDatas[i] = (T)m_gameData[typeof(T)][i];
            }
            return _gameDatas;
        }

        return default(T[]);
    }

}
