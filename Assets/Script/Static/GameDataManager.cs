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
            for(int _dataIndex = 0; _dataIndex < data.Length; _dataIndex++)
            {
                m_gameData[typeof(T)][_dataIndex] = data[_dataIndex];
            }
        }
        else
        {
            IGameData[] _gameData = new IGameData[data.Length];
            for (int _dataIndex = 0; _dataIndex < data.Length; _dataIndex++)
            {
                _gameData[_dataIndex] = data[_dataIndex];
            }
            m_gameData.Add(typeof(T), _gameData);
        }
    }

    public static T GetGameData<T>(int id) where T : IGameData
    {
        if (m_gameData.ContainsKey(typeof(T)))
        {
            for (int _dataIndex = 0; _dataIndex < m_gameData[typeof(T)].Length; _dataIndex++)
            {
                if (m_gameData[typeof(T)][_dataIndex].ID == id)
                {
                    return (T)m_gameData[typeof(T)][_dataIndex];
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
            for (int _dataIndex = 0; _dataIndex < m_gameData[typeof(T)].Length; _dataIndex++)
            {
                _gameDatas[_dataIndex] = (T)m_gameData[typeof(T)][_dataIndex];
            }
            return _gameDatas;
        }

        return default(T[]);
    }

}
