using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI State/Move")]
public class MoveState : AIStateBase {

    [SerializeField] private int m_targetActorID = 0;
    [SerializeField] private int m_aiActorID = 1;

    private Actor m_targetActor = null;
    private AIActor m_aiActor = null;

    public override void Init()
    {
        m_targetActor = ActorManager.GetActor(m_targetActorID);
        m_aiActor = ActorManager.GetActor(m_aiActorID) as AIActor;
        m_aiActor.SetMoveTo(m_targetActor.transform.position);
    }

    public override void Update()
    {
        if(!m_aiActor.IsMoving)
        {
            m_aiActor.SetMoveTo(m_targetActor.transform.position);
        }
    }

}
