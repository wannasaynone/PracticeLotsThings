using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListPage : Page {

    [SerializeField] private List<ActorController> m_characters;

    public event Action<Character> OnCharacterSet;
    public event Action<Character> OnCharacterRemoved;

    public List<Character> Characters
    {
        get
        {
            return new List<Character>(m_createdCharacter);
        }
    }
    public Character Player { get; private set; }
    public List<Character> AI
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
        for(int _characterIndex = 0; _characterIndex < m_createdCharacter.Count; _characterIndex++)
        {
            m_createdCharacter[_characterIndex].Update();
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
        if(_character.CharacterType == Character.Type.Player)
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
        for(int _characterIndex = 0; _characterIndex < m_createdCharacter.Count;) // always remove index 0 until there is no more character in m_createdCharacter
        {
            RemoveCharcterInScene(m_createdCharacter[_characterIndex]);
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

    public Character GetCloestCharacter(Character form, bool isSameCamp)
    {
        List<Character> _characters = new List<Character>(m_createdCharacter);
        _characters.Remove(form);

        if(_characters.Count <= 0)
        {
            return null;
        }

        Character _temp = null;
        for (int _characterIndex = 0; _characterIndex < _characters.Count; _characterIndex++)
        {
            if(isSameCamp && _characters[_characterIndex].Actor.CharacterStatus.Camp == form.Actor.CharacterStatus.Camp
                || !isSameCamp && _characters[_characterIndex].Actor.CharacterStatus.Camp != form.Actor.CharacterStatus.Camp)
            {
                if (_temp == null ||
                    Vector3.Distance(form.Actor.transform.position, _temp.Actor.transform.position) > Vector3.Distance(form.Actor.transform.position, _characters[_characterIndex].Actor.transform.position))
                {
                    _temp = _characters[_characterIndex];
                }
            }
        }

        return _temp;
    }

    public Character GetCloestCharacter(Tower form, bool isSameCamp)
    {
        if (m_createdCharacter.Count <= 0)
        {
            return null;
        }

        Character _temp = null;
        for (int _characterIndex = 0; _characterIndex < m_createdCharacter.Count; _characterIndex++)
        {
            if (isSameCamp && m_createdCharacter[_characterIndex].Actor.CharacterStatus.Camp == form.Camp
                || !isSameCamp && m_createdCharacter[_characterIndex].Actor.CharacterStatus.Camp != form.Camp)
            {
                if (_temp == null ||   
                    Vector3.Distance(form.transform.position, _temp.Actor.transform.position) > Vector3.Distance(form.transform.position, m_createdCharacter[_characterIndex].Actor.transform.position))
                {
                    _temp = m_createdCharacter[_characterIndex];
                }
            }
        }

        return _temp;
    }

}
