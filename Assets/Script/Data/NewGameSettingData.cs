using System;

[Serializable]
public class NewGameSettingData {

    public string GameType { get; private set; }
    public string ActorType { get; private set; }

    public NewGameSettingData(string gameType, string actorType)
    {
        GameType = gameType;
        ActorType = actorType;
    }

}
