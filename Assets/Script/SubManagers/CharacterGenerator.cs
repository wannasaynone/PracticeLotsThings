using UnityEngine;

public class CharacterGenerator : SubManager {

    private CharacterListPage m_characterList;

    public CharacterGenerator(CharacterListPage page) : base(page)
    {
        m_characterList = page;
    }

    public void CreateCharacter(string name, Character.Type characterType, Vector3 bornPosition = default(Vector3), Vector3 rotate = default(Vector3))
    {
        ActorController _character = m_characterList.SetCharacterGameObjectIntoScene(name, characterType);
        _character.transform.position = bornPosition;
        _character.transform.Rotate(rotate);
    }

    public override void Update()
    {
        return;
    }

}
