using UnityEditor;

public class StateNode : BaseNode {

    private BaseState m_state = null;

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

    public override void DrawWindow()
    {
        m_state = EditorGUILayout.ObjectField(m_state, typeof(BaseState), false) as BaseState;

        if(m_state == null)
        {
            EditorGUILayout.HelpBox("Need to assign a state object", MessageType.Error);
            return;
        }
    }
}
