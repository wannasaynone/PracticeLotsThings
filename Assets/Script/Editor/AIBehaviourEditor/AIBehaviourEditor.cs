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


    private static List<BaseNode> m_nodes = new List<BaseNode>();

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

            // TODO: transition link to state...
            _menu.AddItem(new GUIContent("Delete"), false, ContextCallbak, UserAction.DeleteNode);
            _menu.ShowAsContext();
        }
    }

    private long m_currentID = -1;
    private void ContextCallbak(object action)
    {
        switch ((UserAction)action)
        {
            case UserAction.AddState:
                {
                    m_currentID++;
                    StateNode _stateNode = new StateNode(m_currentID);
                    Rect _windowRect = new Rect(new Vector2(m_mousePosition.x, m_mousePosition.y), new Vector2(200, 300));
                    _stateNode.windowRect = _windowRect;
                    m_nodes.Add(_stateNode);
                    m_selectedNode = null;
                    break;
                }
            case UserAction.AddTranstionNode:
                {
                    m_currentID++;
                    TransitionNode _transitionNode = new TransitionNode(m_currentID, (StateNode)m_selectedNode);
                    Rect _windowRect = new Rect(new Vector2(m_mousePosition.x, m_mousePosition.y), new Vector2(200, 150));
                    _transitionNode.windowRect = _windowRect;
                    m_nodes.Add(_transitionNode);
                    ((StateNode)m_selectedNode).transitions_out.Add(_transitionNode);
                    m_selectedNode = null;
                    break;
                }
            case UserAction.DeleteNode:
                {
                    if(m_selectedNode is TransitionNode)
                    {
                        TransitionNode _selectedTranstionNode = m_selectedNode as TransitionNode;

                        if(_selectedTranstionNode.FromStateNode != null)
                        {
                            _selectedTranstionNode.FromStateNode.transitions_out.Remove(_selectedTranstionNode);
                        }

                        if (_selectedTranstionNode.ToStateNode != null)
                        {
                            _selectedTranstionNode.ToStateNode.transitions_in.Remove(_selectedTranstionNode);
                        }
                    }

                    if(m_selectedNode is StateNode)
                    {
                        StateNode _selectedStateNode = m_selectedNode as StateNode;
                        List<TransitionNode> _waitForRemoveTransitionNode = new List<TransitionNode>();
                        for(int i = 0; i < m_nodes.Count; i++)
                        {
                            if(m_nodes[i] is TransitionNode)
                            {
                                TransitionNode _transtionNode = m_nodes[i] as TransitionNode;

                                if (_transtionNode.ToStateNode != null && _transtionNode.ToStateNode.ID == _selectedStateNode.ID)
                                {
                                    _transtionNode.ToStateNode = null;
                                }

                                if (_transtionNode.FromStateNode != null && _transtionNode.FromStateNode.ID == _selectedStateNode.ID)
                                {
                                    _transtionNode.FromStateNode = null;
                                }

                                if(_transtionNode.ToStateNode == null && _transtionNode.FromStateNode == null)
                                {
                                    _waitForRemoveTransitionNode.Add(_transtionNode);
                                }
                            }
                        }

                        for (int i = 0; i < _waitForRemoveTransitionNode.Count; i++)
                        {
                            m_nodes.Remove(_waitForRemoveTransitionNode[i]);
                        }
                    }

                    m_nodes.Remove(m_selectedNode);
                    m_selectedNode = null;
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

    public static void AddNode(BaseNode node)
    {
        if(!m_nodes.Contains(node))
        {
            m_nodes.Add(node);
        }
    }

}
