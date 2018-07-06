using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ConnectionPointType { In, Out }

public class AILogicNodeConnectionPoint {

    public Rect rect;
    public ConnectionPointType type;
    public AILogicNode node;

    // public GUIStyle style;

    public Action<AILogicNodeConnectionPoint> onClickConnectionPoint;

    /*public AILogicNodeConnectionPoint(AILogicNode node, ConnectionPointType type, Action<AILogicNodeConnectionPoint> OnClickConnectionPoint)
    {
        this.node = node;
        this.type = type;
        this.style = style;
        onClickConnectionPoint = OnClickConnectionPoint;
        rect = new Rect(0, 0, 10f, 20f);
    }*/

    public AILogicNodeConnectionPoint(AILogicNode node, ConnectionPointType type, Action<AILogicNodeConnectionPoint> OnClickConnectionPoint)
    {
        this.node = node;
        this.type = type;
        onClickConnectionPoint = OnClickConnectionPoint;
        rect = new Rect(0, 0, 10f, 20f);
    }

    public void Draw()
    {
        rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

        switch (type)
        {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width + 8f;
                break;

            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width - 8f;
                break;
        }

        //if (GUI.Button(rect, "", style))
        if (GUI.Button(rect, ""))
        {
            if (onClickConnectionPoint != null)
            {
                onClickConnectionPoint(this);
            }
        }
    }

}
