using UnityEngine;

public class ZombieActor : Actor {

    public bool IsAttacking { get; protected set; }

    [SerializeField] float m_attackCd = 1f;

    private bool m_lerpBack = false;

    protected override void ParseMotion()
    {
        base.ParseMotion();

        if (m_inputDetecter.StartAttack && m_attackCdTimer < 0f)
        {
            Debug.Log("123");
            m_lerpBack = false;
            m_actorAniamtorController.StartAttackAnimation();
            m_actorAniamtorController.LerpAttackingAnimation(true, 1f);
            m_attackCdTimer = m_attackCd;
            TimerManager.Schedule(2.2f, delegate { m_lerpBack = true; });
        }

        if(m_lerpBack)
        {
            m_actorAniamtorController.LerpAttackingAnimation(false, 0.5f);
        }

        if (m_attackCdTimer > 0)
        {
            m_attackCdTimer -= Time.deltaTime;
        }
    }
}
