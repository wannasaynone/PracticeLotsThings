using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCondition_AllActorDied : IGameOverCondition {

    private int m_shooterNumber = 0;
    private int m_zombieNumber = 0;
    private ActorFilter.ActorType m_playAs = ActorFilter.ActorType.Shooter;

    public GameOverCondition_AllActorDied(ActorFilter.ActorType startAs)
    {
        m_playAs = startAs;
    }

    public bool IsGameOver(Action onIsPlayerWin, Action onIsPlayerLose)
    {
        m_shooterNumber = Engine.ActorFilter.GetActors(new ActorFilter.FilteCondition()
        {
            filteBy = ActorFilter.FilteBy.Type,
            compareCondition = ActorFilter.CompareCondition.Is,
            actorType = ActorFilter.ActorType.Shooter,
            value = 0
        }).Count;

        m_zombieNumber = Engine.ActorFilter.GetActors(new ActorFilter.FilteCondition()
        {
            filteBy = ActorFilter.FilteBy.Type,
            compareCondition = ActorFilter.CompareCondition.Is,
            actorType = ActorFilter.ActorType.Zombie,
            value = 0
        }).Count;

        if(m_playAs == ActorFilter.ActorType.Shooter)
        {
            if (m_shooterNumber <= 0 && m_zombieNumber > 0)
            {
                onIsPlayerLose();
                return true;
            }

            if (m_shooterNumber > 0 && m_zombieNumber <= 0)
            {
                onIsPlayerWin();
                return true;
            }
        }

        if(m_playAs == ActorFilter.ActorType.Zombie)
        {
            if (m_shooterNumber > 0 && m_zombieNumber <= 0)
            {
                onIsPlayerLose();
                return true;
            }

            if (m_shooterNumber <= 0 && m_zombieNumber > 0)
            {
                onIsPlayerWin();
                return true;
            }
        }

        return false;
    }

}
