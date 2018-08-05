using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameSetting {

    public enum GameType
    {
        OneVsOne,
        TwoVsTwo
    }

    public ActorFilter.ActorType startAs = ActorFilter.ActorType.Normal;
    public GameType gameType = GameType.OneVsOne;
    public int normalActorNumber = 50;

}
