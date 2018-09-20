using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameStartCondition : IGameStartCondition {

    private int m_neededShooter = 0;
    private int m_neededZombie = 0;
    private int m_neededNormal = 0;

    public MainGameStartCondition(int neededShooterNumber, int neededZombieNumber, int neededNormalNumber)
    {
        m_neededNormal = neededNormalNumber;
        m_neededShooter = neededShooterNumber;
        m_neededZombie = neededZombieNumber;
    }

    public bool IsGameCanStart()
    {
        if (GetActorNumber(ActorFilter.ActorType.Normal) > m_neededNormal
            || GetActorNumber(ActorFilter.ActorType.Shooter) > m_neededShooter
            || GetActorNumber(ActorFilter.ActorType.Zombie) > m_neededZombie)
        {
            Debug.LogError("Unexcepted error");
            return false;
        }

        return (GetActorNumber(ActorFilter.ActorType.Normal) == m_neededNormal
            && GetActorNumber(ActorFilter.ActorType.Shooter) == m_neededShooter
            && GetActorNumber(ActorFilter.ActorType.Zombie) == m_neededZombie);
    }

    private int GetActorNumber(ActorFilter.ActorType actorType)
    {
       return Engine.ActorFilter.GetActors(new ActorFilter.FilteCondition()
        {
            filteBy = ActorFilter.FilteBy.Type,
            actorType = actorType,
            compareCondition = ActorFilter.CompareCondition.Is,
            value = 0f
        }).Count;
    }

}
