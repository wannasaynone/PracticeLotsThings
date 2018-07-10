using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AI Behaviour/Condition/IsDead")]
public class IsDead : Condition
{
    public override bool CheckCondition(StateManager state)
    {
        return state.health == 0;
    }
}
