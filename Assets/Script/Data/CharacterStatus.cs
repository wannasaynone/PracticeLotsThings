using System;

[Serializable]
public class CharacterStatus : IGameData {

    public static CharacterStatus Default { get { return m_default; } }
    private static CharacterStatus m_default = new CharacterStatus() { HP = 100, Name = "default", ID = -1 };

    public event Action<int> OnHPValueChanged;
    public event Action<int> OnMatValueChanged;

    public int ID { get; private set; }
    public string Name { get; private set; }
    public int HP { get; private set; }
    public int Mat { get; private set; }

    public void SetHP(int value)
    {
        HP = value;
        if(OnHPValueChanged != null)
        {
            OnHPValueChanged(HP);
        }
    }

    public void SetMat(int value)
    {
        Mat = value;
        if(OnMatValueChanged != null)
        {
            OnMatValueChanged(Mat);
        }
    }

    public void AddHP(int value)
    {
        SetHP(HP + value);
    }

    public void AddMat(int value)
    {
        SetMat(Mat + value);
    }

    public CharacterStatus() { }
    public CharacterStatus(CharacterStatus characterStatus)
    {
        ID = characterStatus.ID;
        Name = characterStatus.Name;
        HP = characterStatus.HP;
    }

}
