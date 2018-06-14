using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private ActorController m_player;
    [Header("Main UI")]
    [SerializeField] private CombatPage m_combatPage;

    private List<SubManager> m_subManagers = new List<SubManager>();

    private void Awake()
    {
        m_subManagers.Add(new DamageCalculator(m_combatPage));
    }

    private void Update()
    {
        for (int i = 0; i < m_subManagers.Count; i++)
        {
            m_subManagers[i].Update();
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            m_player.CharacterStatus.HP += 10;
            m_player.CharacterStatus.Attack += 10;
            m_player.CharacterStatus.Defense += 10;
            m_player.CharacterStatus.Material += 10;
        }
    }

    private T GetSubManager<T>() where T : SubManager // use this function to get needed sub manager
    {
        for (int i = 0; i < m_subManagers.Count; i++)
        {
            if (m_subManagers[i].GetType() == typeof(T))
            {
                return (T)m_subManagers[i];
            }
        }

        return default(T);
    }

}
