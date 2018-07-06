using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class AILogicNodeConnection {

    public AILogicNodeConnectionPoint inPoint;
    public AILogicNodeConnectionPoint outPoint;
    public Action<AILogicNodeConnection> onClickRemoveConnection;

    public AILogicNodeConnection(AILogicNodeConnectionPoint inPoint, AILogicNodeConnectionPoint outPoint, Action<AILogicNodeConnection> OnClickRemoveConnection)
    {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        onClickRemoveConnection = OnClickRemoveConnection;
    }

    public void Draw()
    {
        Handles.DrawBezier(
            inPoint.rect.center,
            outPoint.rect.center,
            inPoint.rect.center + Vector2.left * 50f,
            outPoint.rect.center - Vector2.left * 50f,
            Color.white,
            null,
            2f
        );

        if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
        {
            if (onClickRemoveConnection != null)
            {
                onClickRemoveConnection(this);
            }
        }
    }

}
