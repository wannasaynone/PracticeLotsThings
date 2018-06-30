
public class CharacterManager : SubManager {

    private CharacterListPage m_characterList;

    public CharacterManager(CharacterListPage page) : base(page)
    {
        m_characterList = page;
    }

    public void ResetCharacters()
    {
        m_characterList.RemoveAllCharacterInScene();
    }

    public override void Update()
    {
        return;
    }

}
