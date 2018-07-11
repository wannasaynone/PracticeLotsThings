using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AIBehaviourEditor : EditorWindow {

    public enum UserAction
    {
        AddState,
        AddTranstionNode,
        DeleteNode
    }


    private List<BaseNode> m_nodes = new List<BaseNode>();

    private Vector2 m_mousePosition = default(Vector2);
    private BaseNode m_selectedNode = null;

    [MenuItem("Tools/AI Behaviour Editor")]
    private static void Init()
    {
        AIBehaviourEditor editor = GetWindow<AIBehaviourEditor>();
        editor.minSize = new Vector2(800f, 600f);
    }

    private void OnGUI()
    {
        Event e = Event.current;
        m_mousePosition = e.mousePosition;
        DetectInput(e);
        DrawWindows();
    }

    private void DetectInput(Event e)
    {
        if (e.button == 1 && e.type == EventType.MouseDown)
        {
            DetectIsClickedOnNode(e);
            CreateContextMenu();
        }
    }

    private void DetectIsClickedOnNode(Event e)
    {
        for (int _nodeIndex = 0; _nodeIndex < m_nodes.Count; _nodeIndex++)
        {
            if (m_nodes[_nodeIndex].windowRect.Contains(e.mousePosition))
            {
                m_selectedNode = m_nodes[_nodeIndex];
                return;
            }
        }
        m_selectedNode = null;
    }

    private void CreateContextMenu()
    {
        GenericMenu _menu = new GenericMenu();
        if (m_selectedNode == null)
        {
            _menu.AddItem(new GUIContent("Add State Node"), false, ContextCallbak, UserAction.AddState);
            _menu.ShowAsContext();
        }
        else
        {
            if(m_selectedNode is StateNode)
            {
                _menu.AddItem(new GUIContent("Add Transition Node"), false, ContextCallbak, UserAction.AddTranstionNode);
            }

            _menu.AddItem(new GUIContent("Delete"), false, ContextCallbak, UserAction.DeleteNode);
            _menu.ShowAsContext();
        }
    }

    private void ContextCallbak(object action)
    {
        switch ((UserAction)action)
        {
            case UserAction.AddState:
                {
                    StateNode _stateNode = new StateNode();
                    Rect _windowRect = new Rect(new Vector2(m_mousePosition.x, m_mousePosition.y), new Vector2(200, 300));
                    _stateNode.windowRect = _windowRect;
                    m_nodes.Add(_stateNode);
                    m_selectedNode = null;
                    break;
                }
            case UserAction.AddTranstionNode:
                {
                    TransitionNode _transitionNode = new TransitionNode((StateNode)m_selectedNode);
                    Rect _windowRect = new Rect(new Vector2(m_mousePosition.x, m_mousePosition.y), new Vector2(200, 150));
                    _transitionNode.windowRect = _windowRect;
                    m_nodes.Add(_transitionNode);
                    m_selectedNode = null;
                    break;
                }
            case UserAction.DeleteNode:
                {
                    m_nodes.Remove(m_selectedNode);
                    break;
                }
        }
    }

    private void DrawWindows()
    {
        BeginWindows();
        for (int _nodeIndex = 0; _nodeIndex < m_nodes.Count; _nodeIndex++)
        {
            m_nodes[_nodeIndex].windowRect = GUI.Window(_nodeIndex, m_nodes[_nodeIndex].windowRect, DrawNodeWindow, m_nodes[_nodeIndex].Title);
        }
        EndWindows();

        DrawLines();
    }

    private void DrawLines()
    {
        for (int _nodeIndex = 0; _nodeIndex < m_nodes.Count; _nodeIndex++)
        {
            if(m_nodes[_nodeIndex] is TransitionNode)
            {
                TransitionNode _transitionNode = m_nodes[_nodeIndex] as TransitionNode;
                Handles.color = Color.black;
                if (_transitionNode.FromStateNode != null)
                {
                    Handles.DrawLine(_transitionNode.FromStateNode.OutPoint, _transitionNode.EnterPoint);
                }
                
                if(_transitionNode.ToStateNode != null)
                {
                    Handles.DrawLine(_transitionNode.OutPoint, _transitionNode.ToStateNode.EnterPoint);
                }
            }
        }
    }
    
    private void DrawNodeWindow(int id)
    {
        m_nodes[id].DrawWindow();
        GUI.DragWindow();
    }

}
