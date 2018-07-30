using UnityEngine;

public class ZombieActor : Actor {

    public bool IsAttacking { get; protected set; }

    [SerializeField] float m_attackCd = 1f;
    [SerializeField] float m_attackAnimationTime = 2.2f;
    [SerializeField] float m_lerpBackTime = 0.5f;

    private bool m_lerpBack = false;

    protected override void ParseMotion()
    {
        base.ParseMotion();

        if (m_inputDetecter.StartAttack && m_attackCdTimer < 0f)
        {
            m_lerpBack = false;
            m_actorAniamtorController.StartAttackAnimation();
            m_actorAniamtorController.LerpAttackingAnimation(true, 1f, false);
            m_attackCdTimer = m_attackCd;
            TimerManager.Schedule(m_attackAnimationTime, delegate { m_lerpBack = true; });
        }

        if(m_lerpBack)
        {
            m_actorAniamtorController.LerpAttackingAnimation(false, m_lerpBackTime, false);
        }

        if (m_attackCdTimer > 0)
        {
            m_attackCdTimer -= Time.deltaTime;
        }
    }
}
