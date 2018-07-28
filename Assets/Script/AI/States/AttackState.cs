using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI State/Attack")]
public class AttackState : AIStateBase {

    [SerializeField] private int m_targetActorID = 0;
    [SerializeField] private int m_aiActorID = 1;

    private Actor m_targetActor = null;
    private AIActor m_aiActor = null;

    public override void Init()
    {
        m_targetActor = ActorManage.GetActor(m_targetActorID);
        m_aiActor = ActorManage.GetActor(m_aiActorID) as AIActor;
        m_aiActor.StartAttack();
    }

    public override void Update()
    {
        m_aiActor.FaceTo(m_targetActor.transform.position);
    }
}
