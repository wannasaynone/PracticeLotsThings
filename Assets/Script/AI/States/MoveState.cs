﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI State/Move")]
public class MoveState : AIStateBase {

    public enum Target
    {
        Player,
        Nearest
    }

    [SerializeField] private Target m_targetType = Target.Player;
    [SerializeField] private float m_detectRange = 5f;

    private Actor m_ai = null;
    private Actor m_target = null;

    public override void Init(Actor ai)
    {
        m_ai = ai;
        switch (m_targetType)
        {
            case Target.Nearest:
                {
                    List<Actor> _actors = Engine.ActorFilter.GetActors(new ActorFilter.FilteCondition()
                    {
                        filteBy = ActorFilter.FilteBy.Distance,
                        compareCondition = ActorFilter.CompareCondition.Less,
                        actorType = ActorFilter.ActorType.All, //TESTING TODO: need to set type by ai
                        value = m_detectRange
                    });

                    m_target = ActorFilter.GetNearestActor(_actors, m_ai);
                    break;
                }
            case Target.Player:
                {
                    m_target = GameManager.Player;
                    break;
                }
        }
    }

    public override void Update()
    {
        m_ai.ForceMoveTo(m_target.transform.position);
        m_ai.FaceTo(m_target.transform.position);
    }

}
