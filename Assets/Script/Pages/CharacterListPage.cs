using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListPage : Page {

    [SerializeField] private List<ActorController> m_characters;

    public event Action<Character> OnCharacterSet;
    public event Action<Character> OnCharacterRemoved;

    public Character Player { get; private set; }
    public List<Character> Ai
    {
        get
        {
            List<Character> _characters = new List<Character>(m_createdCharacter);
            _characters.Remove(Player);
            return _characters;
        }
    }

    private List<Character> m_createdCharacter = new List<Character>();

    private void Update()
    {
        for(int i = 0; i < m_createdCharacter.Count; i++)
        {
            m_createdCharacter[i].Update();
        }
    }

    public ActorController SetCharacterGameObjectIntoScene(string name, Character.Type characterType)
    {
        if(m_characters == null || m_characters.Count == 0)
        {
            Debug.Log("m_characters == null || m_characters.Count == 0");
            return null;
        }

        ActorController _actor = Instantiate(m_characters.Find(x => x.name == name));
        Character _character = new Character(_actor, characterType);
        if(_character.Info.Type == Character.Type.Player)
        {
            Player = _character;
        }
        m_createdCharacter.Add(_character);

        if(OnCharacterSet != null)
        {
            OnCharacterSet(_character);
        }

        return _actor;
    }

    public void RemoveAllCharacterInScene()
    {
        for(int i = 0; i < m_createdCharacter.Count;) // always remove index 0 until there is no more character in m_createdCharacter
        {
            RemoveCharcterInScene(m_createdCharacter[i]);
        }
    }

    public void RemoveCharcterInScene(Character character)
    {
        m_createdCharacter.Remove(character);
        if (OnCharacterRemoved != null)
        {
            OnCharacterRemoved(character);
        }
    }

}
