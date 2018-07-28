using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StateNode : BaseNode {

    private AIStateBase m_state = null;

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
    private string m_title = "State Node";
    private bool m_inited = false;

    public bool IsNull { get { return m_state == null; } }
    public AIStateBase State { get { return m_state; } }

    public StateNode(long ID) : base(ID) { }

    public override void DrawWindow()
    {
        m_state = EditorGUILayout.ObjectField(m_state, typeof(AIStateBase), false) as AIStateBase;

        if(m_state == null)
        {
            EditorGUILayout.HelpBox("Need to assign a state object", MessageType.Error);
            return;
        }
        else
        {
            if(!m_inited)
            {
                m_inited = true;
            }
        }
    }
}
