using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StateNode : BaseNode {

    public enum StateType
    {
        Idle,
        Attack,
        Move
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

    public List<TransitionNode> transitions_in = new List<TransitionNode>();
    public List<TransitionNode> transitions_out = new List<TransitionNode>();

    private string m_title = "State Node";
    private bool m_inited = false;

    private StateType m_stateType = StateType.Idle;

    public StateNode(long ID) : base(ID) { }

    public override void DrawWindow()
    {
        m_stateType = (StateType)EditorGUILayout.EnumPopup(m_stateType);
    }
}
