using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAniamtorController {

    private const string PARA_NAME_HORIZONTAL = "horizontal";
    private const string PARA_NAME_VERTICAL = "vertical";
    private const string PARA_NAME_MOTION = "motion";
    private const string PARA_NAME_AIMING = "aiming";

    private const string LAYER_NAME_SHOOTING = "Shooting";

    private Actor m_actor = null;
    private Animator m_animator = null;

    private float m_currentWeight_shootingLayer = 0f;

    public ActorAniamtorController(Actor actor, Animator animator)
    {
        m_actor = actor;
        m_animator = animator;
    }

    public void Update()
    {
        m_animator.SetFloat(PARA_NAME_HORIZONTAL, m_actor.HorizontalMotion);
        m_animator.SetFloat(PARA_NAME_VERTICAL, m_actor.VerticalMotion);
        m_animator.SetFloat(PARA_NAME_MOTION, m_actor.MotionCurve);

        if(m_actor.IsShooting)
        {
            m_currentWeight_shootingLayer = Mathf.Lerp(m_currentWeight_shootingLayer, 1f, 0.5f);
            m_animator.SetLayerWeight(m_animator.GetLayerIndex(LAYER_NAME_SHOOTING), m_currentWeight_shootingLayer);
        }
        else
        {
            m_currentWeight_shootingLayer = Mathf.Lerp(m_currentWeight_shootingLayer, 0f, 0.5f);
            m_animator.SetLayerWeight(m_animator.GetLayerIndex(LAYER_NAME_SHOOTING), m_currentWeight_shootingLayer);
        }
    }
}
