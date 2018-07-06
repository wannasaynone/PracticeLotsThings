using UnityEngine;
using UnityEditor;
using System;

public class AILogicNode {

    public bool isDragged;
    public bool isSelected;

    public Rect rect;
    public string title;

    /* public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;*/

    public AILogicNodeConnectionPoint inPoint;
    public AILogicNodeConnectionPoint outPoint;

    public Action<AILogicNode> onRemoveNode;

    /*public AILogicNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<AILogicNodeConnectionPoint> OnClickInPoint, Action<AILogicNodeConnectionPoint> OnClickOutPoint, Action<AILogicNode> OnClickRemoveNode)
    {
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;
        inPoint = new AILogicNodeConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new AILogicNodeConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        onRemoveNode = OnClickRemoveNode;
    }*/

    public AILogicNode(Vector2 position, float width, float height, Action<AILogicNodeConnectionPoint> OnClickInPoint, Action<AILogicNodeConnectionPoint> OnClickOutPoint, Action<AILogicNode> OnClickRemoveNode)
    {
        rect = new Rect(position.x, position.y, width, height);
        inPoint = new AILogicNodeConnectionPoint(this, ConnectionPointType.In, OnClickInPoint);
        outPoint = new AILogicNodeConnectionPoint(this, ConnectionPointType.Out, OnClickOutPoint);
        onRemoveNode = OnClickRemoveNode;
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public void Draw()
    {
        inPoint.Draw();
        outPoint.Draw();
        // GUI.Box(rect, title, style);
        GUI.Box(rect, title);
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                {
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            GUI.changed = true;
                            isSelected = true;
                            // style = selectedNodeStyle;
                        }
                        else
                        {
                            GUI.changed = true;
                            isSelected = false;
                            // style = defaultNodeStyle;
                        }
                    }

                    if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }
                    break;
                }
            case EventType.MouseUp:
                {
                    isDragged = false;
                    break;
                }
            case EventType.MouseDrag:
                {
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
                }
        }
        return false;
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        if (onRemoveNode != null)
        {
            onRemoveNode(this);
        }
    }

}
