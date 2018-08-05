using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager {

    public static Actor Player { get { return m_player; } }
    private static Actor m_player = null;

    private GameSetting m_gameSetting = null;
    private ActorManager m_actorManager = null;

    public GameManager(ActorManager actorManager, GameSetting gameSetting)
    {
        m_actorManager = actorManager;
        m_gameSetting = gameSetting;
    }

    public void InitGame(NewGameSetting newGameSetting)
    {
        switch(newGameSetting.startAs)
        {
            case ActorFilter.ActorType.Shooter:
                {
                    m_player = m_actorManager.CreateActor(m_gameSetting.ShooterActorPrefabID);
                    break;
                }
            case ActorFilter.ActorType.Zombie:
                {
                    m_player = m_actorManager.CreateActor(m_gameSetting.ZombieActorPrefabID);
                    break;
                }
        }

        m_player.EnableAI(false);
        CameraController.MainCameraController.Track(m_player.gameObject);

        // TODO: network...
        switch (newGameSetting.gameType)
        {
            case NewGameSetting.GameType.OneVsOne:
                {
                    switch (newGameSetting.startAs)
                    {
                        case ActorFilter.ActorType.Shooter:
                            {
                                m_actorManager.CreateActor(m_gameSetting.ZombieActorPrefabID, Engine.GetRamdomPosition()).EnableAI(true);
                                break;
                            }
                        case ActorFilter.ActorType.Zombie:
                            {
                                m_actorManager.CreateActor(m_gameSetting.ShooterActorPrefabID, Engine.GetRamdomPosition()).EnableAI(true);
                                break;
                            }
                    }
                    break;
                }
                // TODO: AI setting when 2V2 (need to write conditions...)
            case NewGameSetting.GameType.TwoVsTwo:
                {
                    switch (newGameSetting.startAs)
                    {
                        case ActorFilter.ActorType.Shooter:
                            {
                                m_actorManager.CreateActor(m_gameSetting.ZombieActorPrefabID, Engine.GetRamdomPosition()).EnableAI(true);
                                m_actorManager.CreateActor(m_gameSetting.ZombieActorPrefabID, Engine.GetRamdomPosition()).EnableAI(true);
                                m_actorManager.CreateActor(m_gameSetting.ShooterActorPrefabID, Engine.GetRamdomPosition()).EnableAI(true);
                                break;
                            }
                        case ActorFilter.ActorType.Zombie:
                            {
                                m_actorManager.CreateActor(m_gameSetting.ShooterActorPrefabID, Engine.GetRamdomPosition()).EnableAI(true);
                                m_actorManager.CreateActor(m_gameSetting.ShooterActorPrefabID, Engine.GetRamdomPosition()).EnableAI(true);
                                m_actorManager.CreateActor(m_gameSetting.ZombieActorPrefabID, Engine.GetRamdomPosition()).EnableAI(true);
                                break;
                            }
                    }
                    break;
                }
        }

        for(int i = 0; i < newGameSetting.normalActorNumber; i++)
        {
            m_actorManager.CreateActor(m_gameSetting.NormalActorPrefabID, Engine.GetRamdomPosition());
        }

    }

}
