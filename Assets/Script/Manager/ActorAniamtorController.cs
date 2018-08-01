using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAniamtorController {

    private const string PARA_NAME_HORIZONTAL = "horizontal";
    private const string PARA_NAME_VERTICAL = "vertical";
    private const string PARA_NAME_MOTION = "motion";

    private const string CLIP_NAME_ATTACKING = "Attacking";

    private const string LAYER_NAME_ATTCKING_HANDS = "Attacking_Hands";
    private const string LAYER_NAME_ATTACKING_FULL_BODY = "Attacking_FullBody";

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

    public void ForceRestartAttackAnimation()
    {
        m_animator.Play(CLIP_NAME_ATTACKING, m_animator.GetLayerIndex(LAYER_NAME_ATTCKING_HANDS), 0f);
        m_animator.Play(CLIP_NAME_ATTACKING, m_animator.GetLayerIndex(LAYER_NAME_ATTACKING_FULL_BODY), 0f);
    }

    public void LerpAttackingAnimation(bool isAttacking, float lerpT, bool onlyHands)
    {
        if(isAttacking)
        {
            m_currentWeight_shootingLayer = Mathf.Lerp(m_currentWeight_shootingLayer, 1f, lerpT);
        }
        else
        {
            m_currentWeight_shootingLayer = Mathf.Lerp(m_currentWeight_shootingLayer, 0f, lerpT);
        }

        if(onlyHands)
        {
            m_animator.SetLayerWeight(m_animator.GetLayerIndex(LAYER_NAME_ATTCKING_HANDS), m_currentWeight_shootingLayer);
        }
        else
        {
            m_animator.SetLayerWeight(m_animator.GetLayerIndex(LAYER_NAME_ATTACKING_FULL_BODY), m_currentWeight_shootingLayer);
        }
    }
}
