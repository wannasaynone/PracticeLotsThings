using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController_ShooterActor : ActorController {

    [SerializeField] private float m_rollDetectTime = 0.1f;

    private ShooterActor m_shooterActor = null;
    private float m_rollDetectTimer = -1f;

    private string m_lastDirectionButton = "";

    protected virtual void Start()
    {
        if (m_actor is ShooterActor)
        {
            m_shooterActor = m_actor as ShooterActor;
        }
        else
        {
            Debug.LogError("MUST set a ShooterActor in ActorController_ShooterActor");
            m_shooterActor = null;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (m_inputDetecter.IsMovePressed && m_rollDetectTimer > 0 && m_inputDetecter.LastDirectionButton == m_lastDirectionButton)
        {
            ((ShooterActor)m_actor).StartRoll(new Vector3(m_inputDetecter.Horizontal, 0, m_inputDetecter.Vertical));
        }

        if (m_inputDetecter.IsMovePressed && m_rollDetectTimer < 0)
        {
            m_rollDetectTimer = m_rollDetectTime;
            m_lastDirectionButton = m_inputDetecter.LastDirectionButton;
        }

        if (m_rollDetectTimer > 0)
        {
            m_rollDetectTimer -= Time.deltaTime;
        }

        if (m_inputDetecter.IsStartingAttack)
        {
            m_shooterActor.StartAttack();
        }
        if(!m_inputDetecter.IsAttacking)
        {
            m_shooterActor.StopAttack();
        }
    }

}
