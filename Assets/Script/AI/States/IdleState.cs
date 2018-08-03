using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI State/Idle")]
public class IdleState : AIStateBase {

    public override void Init(Actor ai)
    {
        base.Init(ai);
        m_aiActor.ForceIdle();
    }

    public override void Update()
    {
        // do nothing when idle
        return;
    }

}
