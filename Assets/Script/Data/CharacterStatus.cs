using System;

[Serializable]
public class CharacterStatus : IGameData {

    public event Action<int> OnHPValueChanged;

    public int ID { get; private set; }
    public string Name { get; private set; }
    public int HP { get; private set; }

    public void SetHP(int value)
    {
        HP = value;
        if(OnHPValueChanged != null)
        {
            OnHPValueChanged(HP);
        }
    }

    public void AddHP(int value)
    {
        SetHP(HP + value);
    }

    public CharacterStatus() { }
    public CharacterStatus(CharacterStatus characterStatus)
    {
        ID = characterStatus.ID;
        Name = characterStatus.Name;
        HP = characterStatus.HP;
    }

}
