using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIBehaviourData : ScriptableObject {

    public List<AIStateBase> aiStates = new List<AIStateBase>();

}
