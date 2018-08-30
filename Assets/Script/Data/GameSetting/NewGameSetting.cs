using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Setting/New Game Setting")]
public class NewGameSetting : ScriptableObject {

    private const string GAME_TYPE_STRING_PVP = "pvp_";
    private const string GAME_TYPE_STRING_PVE = "pve_";
    private const string GAME_TYPE_STRING_1V1 = "1v1";
    private const string GAME_TYPE_STRING_2V2 = "2v2";

    private const string ACTOR_TYPE_ZOMBIE = "Zombie";
    private const string ACTOR_TYPE_SHOOTER = "Shooter";

    public enum GameType
    {
        PvP,
        PvE,
    }

    public enum PlayerNumber
    {
        _1v1,
        _2v2
    }

    public ActorFilter.ActorType startAs = ActorFilter.ActorType.Normal;
    public GameType gameType = GameType.PvE;
    public PlayerNumber playerNumber = PlayerNumber._1v1;
    public int normalActorNumber = 50;

    public string GetGameTypeString()
    {
        return GetTypeString() + GetNumberString();
    }

    public string GetActorString()
    {
        return startAs == ActorFilter.ActorType.Shooter ? ACTOR_TYPE_SHOOTER : ACTOR_TYPE_ZOMBIE;
    }

    private string GetTypeString()
    {
        switch (gameType)
        {
            case GameType.PvE:
                return GAME_TYPE_STRING_PVE;
            case GameType.PvP:
                return GAME_TYPE_STRING_PVP;
        }
        return "";
    }

    private string GetNumberString()
    {
        switch (playerNumber)
        {
            case PlayerNumber._1v1:
                return GAME_TYPE_STRING_1V1;
            case PlayerNumber._2v2:
                return GAME_TYPE_STRING_2V2;
        }
        return "";
    }

}
