using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Setting/New Game Setting")]
public class NewGameSetting : ScriptableObject {

    public enum GameType
    {
        PvP_1v1,
        PvP_2v2,
        PvE_1v1,
        PvE_2v2
    }

    public ActorFilter.ActorType startAs = ActorFilter.ActorType.Normal;
    public GameType gameType = GameType.PvE_1v1;
    public int normalActorNumber = 50;

}
