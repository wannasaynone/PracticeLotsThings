using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Setting/New Game Setting")]
public class NewGameSetting : ScriptableObject {

    public enum GameType
    {
        OneVsOne,
        TwoVsTwo
    }

    public ActorFilter.ActorType startAs = ActorFilter.ActorType.Normal;
    public GameType gameType = GameType.OneVsOne;
    public int normalActorNumber = 50;

}
