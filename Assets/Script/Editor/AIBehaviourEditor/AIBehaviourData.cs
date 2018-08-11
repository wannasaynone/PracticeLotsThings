using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIBehaviourData : ScriptableObject {

    public string savePath = "";
    public List<BaseNode> nodeDatas = new List<BaseNode>();

}
