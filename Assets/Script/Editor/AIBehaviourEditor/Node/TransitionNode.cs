using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TransitionNodeData
{
    public enum ConditionType
    {
        Distance,
        NearestIs,
        Status
    }

    public ConditionType conditionType = ConditionType.NearestIs;

    public ActorFilter.ActorType actorType = ActorFilter.ActorType.Normal;
    public AIConditionBase.CompareCondition compareCondition = AIConditionBase.CompareCondition.Less;
    public AIConditionBase.StatusType statusType = AIConditionBase.StatusType.HP;

    public DistanceCondition.Target distanceConditionTarget = DistanceCondition.Target.NearestNormal;
    public StatusCondition.Target statusConditionTarget = StatusCondition.Target.NearestNormal;
    public float distance = 0f;
    public int value = 0;
}

[System.Serializable]
public class TransitionNode : BaseNode {

    public StateNode FromStateNode = null;
    public StateNode ToStateNode = null;

    public List<TransitionNodeData> transitionNodeDatas = new List<TransitionNodeData>();

    public float orgainHeight = 0f;

    public override string Title
    {
        get
        {
            return m_title;
        }

        set
        {
            m_title = value;
        }
    }

    private string m_title = "Transition Node";

    public override void DrawWindow()
    {
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("+"))
        {
            if(transitionNodeDatas == null)
            {
                transitionNodeDatas = new List<TransitionNodeData>();
            }
            transitionNodeDatas.Add(new TransitionNodeData());
        }

        if (GUILayout.Button("-"))
        {
            if (transitionNodeDatas == null || transitionNodeDatas.Count <= 0)
            {
                return;
            }
            transitionNodeDatas.RemoveAt(transitionNodeDatas.Count - 1);
        }
        EditorGUILayout.EndHorizontal();
        if (transitionNodeDatas != null)
        {
            float _finalHeight = orgainHeight;
            for(int i = 0; i < transitionNodeDatas.Count; i++)
            {
                EditorGUILayout.BeginVertical();
                transitionNodeDatas[i].conditionType = (TransitionNodeData.ConditionType)EditorGUILayout.EnumPopup(transitionNodeDatas[i].conditionType);
                switch (transitionNodeDatas[i].conditionType)
                {
                    case TransitionNodeData.ConditionType.Distance:
                        {
                            transitionNodeDatas[i].distanceConditionTarget = (DistanceCondition.Target)EditorGUILayout.EnumPopup("Target:", transitionNodeDatas[i].distanceConditionTarget);
                            transitionNodeDatas[i].compareCondition = (AIConditionBase.CompareCondition)EditorGUILayout.EnumPopup("Condition:", transitionNodeDatas[i].compareCondition);
                            transitionNodeDatas[i].distance = EditorGUILayout.FloatField("Distance:", transitionNodeDatas[i].distance);
                            _finalHeight += 80f;
                            break;
                        }
                    case TransitionNodeData.ConditionType.NearestIs:
                        {
                            transitionNodeDatas[i].actorType = (ActorFilter.ActorType)EditorGUILayout.EnumPopup("Actor Type:", transitionNodeDatas[i].actorType);
                            _finalHeight += 30f;
                            break;
                        }
                    case TransitionNodeData.ConditionType.Status:
                        {
                            transitionNodeDatas[i].statusConditionTarget = (StatusCondition.Target)EditorGUILayout.EnumPopup("Target:", transitionNodeDatas[i].statusConditionTarget);
                            transitionNodeDatas[i].statusType = (AIConditionBase.StatusType)EditorGUILayout.EnumPopup("Status:", transitionNodeDatas[i].statusType);
                            transitionNodeDatas[i].compareCondition = (AIConditionBase.CompareCondition)EditorGUILayout.EnumPopup("Condition:", transitionNodeDatas[i].compareCondition);
                            transitionNodeDatas[i].value = EditorGUILayout.IntField("Value:", transitionNodeDatas[i].value);
                            _finalHeight += 100f;
                            break;
                        }
                }
                EditorGUILayout.EndVertical();
            }
            windowRect.height = _finalHeight;
        }
    }

}
