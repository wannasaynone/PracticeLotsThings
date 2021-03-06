﻿using System.Collections.Generic;
using UnityEditor;
using PracticeLotsThings.AI;

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

    public StateType stateType = StateType.Idle;

    public int defaultIdleStateNodeID = -1;
    public AttackState.Target attackTargetType = AttackState.Target.NearestNormal;
    public MoveState.Target moveTargetType = MoveState.Target.Random;
    public float detctRangeData = 0f;

    public override void DrawWindow()
    {
        EditorGUILayout.LabelField("Node ID:" + ID);
        stateType = (StateType)EditorGUILayout.EnumPopup(stateType);
        switch(stateType)
        {
            case StateType.Idle:
                {
                    break;
                }
            case StateType.Attack:
                {
                    attackTargetType = (AttackState.Target)EditorGUILayout.EnumPopup("Target:", attackTargetType);
                    defaultIdleStateNodeID = EditorGUILayout.IntField("Idle State Node ID:", defaultIdleStateNodeID);
                    break;
                }
            case StateType.Move:
                {
                    moveTargetType = (MoveState.Target)EditorGUILayout.EnumPopup("Target:", moveTargetType);
                    defaultIdleStateNodeID = EditorGUILayout.IntField("Idle State Node ID:", defaultIdleStateNodeID);
                    detctRangeData = EditorGUILayout.FloatField("Detect Range:", detctRangeData);
                    break;
                }
        }
    }
}
