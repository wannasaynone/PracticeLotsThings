using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TransitionNode : BaseNode {

    public StateNode FromStateNode;
    public StateNode ToStateNode;

    // public TransitionNode(long ID, StateNode form, Condition condition = null) : base(ID)
    // {
    //     FromStateNode = form;
    //     m_condition = null;
    // }

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
        // m_condition = EditorGUILayout.ObjectField(m_condition, typeof(Condition), false) as Condition;

        // if (m_condition == null)
        // {
        //     EditorGUILayout.HelpBox("Need to assign a condition object", MessageType.Error);
        //     return;
        // }
    }

}
