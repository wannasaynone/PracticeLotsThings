using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStatus : IGameData {

    public int ID { get; private set; }
    public string Name { get; private set; }
    public int HP;

    public CharacterStatus() { }
    public CharacterStatus(CharacterStatus characterStatus)
    {
        ID = characterStatus.ID;
        Name = characterStatus.Name;
        HP = characterStatus.HP;
    }

}
