using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListPage : Page {

    [SerializeField] private List<ActorController> m_characters;

    public event Action<Character> OnCharacterSet;

    private List<Character> m_createdCharacter = new List<Character>();

    private void Update()
    {
        for(int i = 0; i < m_createdCharacter.Count; i++)
        {
            m_createdCharacter[i].Update();
        }
    }

    public ActorController SetCharacterGameObjectIntoScene(string name)
    {
        if(m_characters == null || m_characters.Count == 0)
        {
            Debug.Log("m_characters == null || m_characters.Count == 0");
            return null;
        }

        ActorController _actor = Instantiate(m_characters.Find(x => x.name == name));
        Character _character = new Character(_actor);
        m_createdCharacter.Add(_character);

        if(OnCharacterSet != null)
        {
            OnCharacterSet(_character);
        }

        return _actor;
    }

    public void RemoveAllCharacterInScene()
    {
        for(int i = 0; i < m_createdCharacter.Count; i++)
        {
            m_createdCharacter[i].RemoveCharacter();
        }
        m_createdCharacter.Clear();
    }

}
