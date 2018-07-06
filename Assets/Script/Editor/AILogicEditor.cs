using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AILogicEditor : EditorWindow {

    private List<AILogicNode> m_nodes;
    private List<AILogicNodeConnection> m_connections;

    /*private GUIStyle m_nodeStyle;
    private GUIStyle m_selectedNodeStyle;
    private GUIStyle m_inPointStyle;
    private GUIStyle m_outPointStyle;*/

    private AILogicNodeConnectionPoint m_selectedInPoint;
    private AILogicNodeConnectionPoint m_selectedOutPoint;

    private Vector2 m_offset;
    private Vector2 m_drag;

    [MenuItem("Window/Node editor")]
    private static void ShowEditor()
    {
        AILogicEditor editor = GetWindow<AILogicEditor>();
    }

    /*private void OnEnable()
    {
        m_nodeStyle = new GUIStyle();
        m_nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        m_nodeStyle.border = new RectOffset(12, 12, 12, 12);

        m_selectedNodeStyle = new GUIStyle();
        m_selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        m_selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        m_inPointStyle = new GUIStyle();
        m_inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        m_inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        m_inPointStyle.border = new RectOffset(4, 4, 12, 12);

        m_outPointStyle = new GUIStyle();
        m_outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        m_outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        m_outPointStyle.border = new RectOffset(4, 4, 12, 12);
    }*/

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        DrawNodes();
        DrawConnections();

        DrawConnectionLine(Event.current);

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        m_offset += m_drag * 0.5f;
        Vector3 newOffset = new Vector3(m_offset.x % gridSpacing, m_offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawNodes()
    {
        if (m_nodes != null)
        {
            for (int i = 0; i < m_nodes.Count; i++)
            {
                m_nodes[i].Draw();
            }
        }
    }

    private void DrawConnections()
    {
        if (m_connections != null)
        {
            for (int i = 0; i < m_connections.Count; i++)
            {
                m_connections[i].Draw();
            }
        }
    }

    private void DrawConnectionLine(Event e)
    {
        if (m_selectedInPoint != null && m_selectedOutPoint == null)
        {
            Handles.DrawBezier(
                m_selectedInPoint.rect.center,
                e.mousePosition,
                m_selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (m_selectedOutPoint != null && m_selectedInPoint == null)
        {
            Handles.DrawBezier(
                m_selectedOutPoint.rect.center,
                e.mousePosition,
                m_selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    private void ProcessEvents(Event e)
    {
        m_drag = Vector2.zero;
        switch (e.type)
        {
            case EventType.MouseDown:
                {
                    if (e.button == 0)
                    {
                        ClearConnectionSelection();
                    }

                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }
                    break;
                }
            case EventType.MouseDrag:
                {
                    if (e.button == 0)
                    {
                        OnDrag(e.delta);
                    }
                    break;
                }
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
        genericMenu.ShowAsContext();
    }

    private void ProcessNodeEvents(Event e)
    {
        if (m_nodes != null)
        {
            for (int i = m_nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = m_nodes[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    private void OnClickAddNode(Vector2 mousePosition)
    {
        if (m_nodes == null)
        {
            m_nodes = new List<AILogicNode>();
        }

        // m_nodes.Add(new AILogicNode(mousePosition, 200, 50, m_nodeStyle, m_selectedNodeStyle, m_inPointStyle, m_outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
        m_nodes.Add(new AILogicNode(mousePosition, 200, 50, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
    }

    private void OnClickInPoint(AILogicNodeConnectionPoint inPoint)
    {
        m_selectedInPoint = inPoint;

        if (m_selectedInPoint != null)
        {
            if (m_selectedOutPoint.node != m_selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickOutPoint(AILogicNodeConnectionPoint outPoint)
    {
        m_selectedOutPoint = outPoint;

        if (m_selectedInPoint != null)
        {
            if (m_selectedOutPoint.node != m_selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickRemoveConnection(AILogicNodeConnection connection)
    {
        m_connections.Remove(connection);
    }

    private void CreateConnection()
    {
        if (m_connections == null)
        {
            m_connections = new List<AILogicNodeConnection>();
        }

        m_connections.Add(new AILogicNodeConnection(m_selectedInPoint, m_selectedOutPoint, OnClickRemoveConnection));
    }

    private void ClearConnectionSelection()
    {
        m_selectedInPoint = null;
        m_selectedOutPoint = null;
    }

    private void OnClickRemoveNode(AILogicNode node)
    {
        if (m_connections != null)
        {
            List<AILogicNodeConnection> _connectionsToRemove = new List<AILogicNodeConnection>();

            for (int i = 0; i < m_connections.Count; i++)
            {
                if (m_connections[i].inPoint == node.inPoint || m_connections[i].outPoint == node.outPoint)
                {
                    _connectionsToRemove.Add(m_connections[i]);
                }
            }

            for (int i = 0; i < _connectionsToRemove.Count; i++)
            {
                m_connections.Remove(_connectionsToRemove[i]);
            }

            _connectionsToRemove = null;
        }

        m_nodes.Remove(node);
    }

    private void OnDrag(Vector2 delta)
    {
        m_drag = delta;

        if (m_nodes != null)
        {
            for (int i = 0; i < m_nodes.Count; i++)
            {
                m_nodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }

}