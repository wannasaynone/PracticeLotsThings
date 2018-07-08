using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentNode : BaseNode {

    private string m_comment = "Testing";

    public override void DrawWindow()
    {
        base.DrawWindow();
        m_comment = GUILayout.TextArea(m_comment, 200);
    }

}
