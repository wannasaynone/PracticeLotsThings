using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Setting/Game Setting")]
public class GameSetting : ScriptableObject {

    public int NormalActorPrefabID { get { return m_normalActorPrefabId; } }
    public int ShooterActorPrefabID { get { return m_shooterActorPrefabId; } }
    public int ZombieActorPrefabID { get { return m_zombieActorPrefabId; } }

    [SerializeField] private int m_normalActorPrefabId = 0;
    [SerializeField] private int m_shooterActorPrefabId = 1;
    [SerializeField] private int m_zombieActorPrefabId = 2;

}
