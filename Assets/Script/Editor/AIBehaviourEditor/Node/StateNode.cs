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

    private int m_defaultIdleStateNodeID = -1;
    private AttackState.Target m_attackTargetType = AttackState.Target.NearestNormal;

    public StateNode(long ID) : base(ID) { }

    public override void DrawWindow()
    {
        EditorGUILayout.LabelField("Node ID:" + ID);
        m_stateType = (StateType)EditorGUILayout.EnumPopup(m_stateType);
        switch(m_stateType)
        {
            case StateType.Idle:
                {
                    break;
                }
            case StateType.Attack:
                {
                    m_attackTargetType = (AttackState.Target)EditorGUILayout.EnumPopup("Target:", m_attackTargetType);
                    m_defaultIdleStateNodeID = EditorGUILayout.IntField("Idle State Node ID:", m_defaultIdleStateNodeID);
                    break;
                }
        }
    }
}
