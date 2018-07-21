using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAniamtorController {

    private const string ANIMATOR_PARA_HORIZONTAL = "horizontal";
    private const string ANIMATOR_PARA_VERTICAL = "vertical";
    private const string ANIMATOR_PARA_MOTION = "motion";

    private Actor m_actor = null;
    private Animator m_animator = null;

    public ActorAniamtorController(Actor actor, Animator animator)
    {
        m_actor = actor;
        m_animator = animator;
    }

    public void Update()
    {
        m_animator.SetFloat(ANIMATOR_PARA_HORIZONTAL, m_actor.HorizontalMotion);
        m_animator.SetFloat(ANIMATOR_PARA_VERTICAL, m_actor.VerticalMotion);
        m_animator.SetFloat(ANIMATOR_PARA_MOTION, m_actor.MotionCurve);
    }

}
