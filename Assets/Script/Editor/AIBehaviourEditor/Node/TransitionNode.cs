using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TransitionNode : BaseNode {

    public enum ConditionType
    {
        Distance,
        NearestIs,
        Status
    }

    public StateNode FromStateNode;
    public StateNode ToStateNode;

    private ConditionType m_conditionType = ConditionType.NearestIs;

    private ActorFilter.ActorType m_actorType = ActorFilter.ActorType.Normal;

    public TransitionNode(long ID, StateNode form) : base(ID)
    {
        FromStateNode = form;
    }

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
        m_conditionType = (ConditionType)EditorGUILayout.EnumPopup(m_conditionType);
        switch (m_conditionType)
        {
            case ConditionType.Distance:
                {
                    break;
                }
            case ConditionType.NearestIs:
                {
                    m_actorType = (ActorFilter.ActorType)EditorGUILayout.EnumPopup("Actor Type:", m_actorType);
                    break;
                }
            case ConditionType.Status:
                {
                    break;
                }
        }
    }

}
