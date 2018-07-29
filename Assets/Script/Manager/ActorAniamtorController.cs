using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAniamtorController {

    private const string PARA_NAME_HORIZONTAL = "horizontal";
    private const string PARA_NAME_VERTICAL = "vertical";
    private const string PARA_NAME_MOTION = "motion";

    private const string CLIP_NAME_ATTACKING = "Attacking";

    private const string LAYER_NAME_ATTCKING = "Attacking";

    private Animator m_animator = null;

    private float m_currentWeight_shootingLayer = 0f;

    public ActorAniamtorController(Animator animator)
    {
        m_animator = animator;
    }

    public void SetMovementAniamtion(float horizontal, float vertical, float motion)
    {
        m_animator.SetFloat(PARA_NAME_HORIZONTAL, horizontal);
        m_animator.SetFloat(PARA_NAME_VERTICAL, vertical);
        m_animator.SetFloat(PARA_NAME_MOTION, motion);
    }

    public void StartAttackAnimation()
    {
        m_animator.Play(CLIP_NAME_ATTACKING, m_animator.GetLayerIndex(LAYER_NAME_ATTCKING), 0f);
    }

    public void LerpAttackingAnimation(bool isAttacking, float lerpT)
    {
        if(isAttacking)
        {
            m_currentWeight_shootingLayer = Mathf.Lerp(m_currentWeight_shootingLayer, 1f, lerpT);
        }
        else
        {
            m_currentWeight_shootingLayer = Mathf.Lerp(m_currentWeight_shootingLayer, 0f, lerpT);
        }
        m_animator.SetLayerWeight(m_animator.GetLayerIndex(LAYER_NAME_ATTCKING), m_currentWeight_shootingLayer);
    }
}
