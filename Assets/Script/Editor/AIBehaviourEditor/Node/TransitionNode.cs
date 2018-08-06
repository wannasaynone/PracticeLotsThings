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
    }

}
