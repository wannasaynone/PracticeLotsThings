using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StateNode : BaseNode {

    public State currentState;

    public override void DrawWindow()
    {
        base.DrawWindow();
        currentState = EditorGUILayout.ObjectField(currentState, typeof(State), false) as State;
    }

    public override void DrawCurve()
    {
        base.DrawCurve();
    }

}
