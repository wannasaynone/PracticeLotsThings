using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TransitionNode : BaseNode {

    public Transition targetTransition;
    public StateNode enterState;
    public StateNode targetState;

    public void Init(StateNode enter, Transition transition)
    {
        enterState = enter;
        targetTransition = transition;
    }

    public override void DrawWindow()
    {
        base.DrawWindow();
        if(targetTransition == null)
        {
            return;
        }
        EditorGUILayout.LabelField("");
        targetTransition.condition = (Condition)EditorGUILayout.ObjectField(targetTransition.condition, typeof(Condition), false);

        if(targetTransition.condition == null)
        {
            EditorGUILayout.HelpBox("No Condition", MessageType.Error);
        }
        else
        {
            targetTransition.disable = EditorGUILayout.Toggle("Disable", targetTransition.disable);
        }
    }

    public override void DrawCurve()
    {
        base.DrawCurve();
        if(enterState)
        {
            Rect _rect = windowRect;
            _rect.y += windowRect.height * 0.5f;
            _rect.width = 1;
            _rect.height = 1;

            AIBehaviourEditor.DrawNodeCurve(enterState.windowRect, _rect, true);
        }
    }

}
